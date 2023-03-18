using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Models.Hotels.Users;
using Microsoft.AspNetCore.Identity;
//using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;

namespace HotelListing.API.Repository
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthManager> _logger;
        private ApiUser _user;
        private const string _loginProvider = "HotelListingApi";
        private const string _refreshToken = "RefreshToken";

        public AuthManager(IMapper mapper,UserManager<ApiUser> userManager,IConfiguration configuration, ILogger<AuthManager> logger)
        {
            this._mapper = mapper;
            this._userManager = userManager;
            this._configuration = configuration;
            this._logger = logger;
        }

        public async Task<AuthResponseDetails> Login(Login login)
        {
            _logger.LogInformation($"Looking for user with email {login.Email}.");
            _user = await _userManager.FindByEmailAsync(login.Email);
            bool isValidUser = await _userManager.CheckPasswordAsync(_user, login.Password);
            if(_user == null || isValidUser == false)
            {
                _logger.LogWarning($"User with email {login.Email} Not found.");

                return null;
            }
            var token = await GenerateToken();
            _logger.LogInformation($"Token Genarated for user with email {login.Email} |Token :{token}.");
            return new AuthResponseDetails
            {
                Token = token,
                UserId = _user.Id,
                RefreshToken = await CreateRefreshToken()
            };
        }

        public async Task<IEnumerable<IdentityError>> Register(ApiUserDetails apiUserDtails)
        {
            _user =_mapper.Map<ApiUser>(apiUserDtails);
            _user.UserName = apiUserDtails.Email;

            var result = await _userManager.CreateAsync(_user, apiUserDtails.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(_user, "User");
            }
            return result.Errors;
        }

        public async Task<string> GenerateToken()
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var roles = await _userManager.GetRolesAsync(_user);

            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList(); 
            var userClaims = await _userManager.GetClaimsAsync(_user);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,_user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,_user.Email),
                new Claim("uid",_user.Id),
            }.Union(userClaims).Union(roleClaims);

            var token = new JwtSecurityToken(
                 issuer: _configuration["JwtSettings:Issuer"],
                 audience: _configuration["JwtSettings:Audience"],
                 claims: claims,
                 expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                 signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> CreateRefreshToken()
        {
            await _userManager.RemoveAuthenticationTokenAsync(_user, _loginProvider, _refreshToken);
            var newRefreshToken=await _userManager.GenerateUserTokenAsync(_user, _loginProvider, _refreshToken);
            var result=await _userManager.SetAuthenticationTokenAsync(_user, _loginProvider, _refreshToken, newRefreshToken);
            return newRefreshToken;
        }

        public async Task<AuthResponseDetails> VerifyRefreshToken(AuthResponseDetails request)
        {
            var jwtSecurityTokenHandler  = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
            var username =tokenContent.Claims.ToList().FirstOrDefault(q=>q.Type== JwtRegisteredClaimNames.Email)?.Value;
            _user = await _userManager.FindByNameAsync(username);
            if (_user == null || _user.Id!=request.UserId)
            {
                return null;
            }
            var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(_user, _loginProvider, _refreshToken, request.RefreshToken);
            if (isValidRefreshToken)
            {
                var token = await GenerateToken();
                return new AuthResponseDetails
                {
                    Token = token,
                    UserId = _user.Id,
                    RefreshToken = await CreateRefreshToken()
                };
            }
            await _userManager.UpdateSecurityStampAsync(_user);
            return null;
        }
    }
}
