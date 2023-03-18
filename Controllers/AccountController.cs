using HotelListing.API.Contracts;
using HotelListing.API.Models.Hotels.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly ILogger _logger;

        public AccountController(IAuthManager authManager , ILogger<AccountController> logger)
        {
            this._authManager = authManager;
            this._logger = logger;
        }
        //POST: api/accout/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Register ([FromBody] ApiUserDetails apiUserDetails)
        {
            try
            {
                _logger.LogInformation($"Registration Attempt for {apiUserDetails.Email}");
                var errors = await _authManager.Register(apiUserDetails);
                if (errors.Any())
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"Something went wrong in {nameof(Register)} -- user Registration Attempt for {apiUserDetails.Email}");
                return Problem($"Something went wrong in {nameof(Register)}. Please contact support", statusCode: 500);
            }
           
        }
        //POST: api/accout/login
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Login([FromBody] Login login)
        {
            try
            {
                _logger.LogInformation($"Registration Attempt for {login.Email}");
                var authResponse = await _authManager.Login(login);
                if (authResponse == null)
                {
                    return Unauthorized();
                }
                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in {nameof(Login)} -- user Login Attempt for {login.Email}");
                return Problem($"Something went wrong in {nameof(Login)}. Please contact support", statusCode: 500);
            }
        }

        //POST: api/accout/refresh
        [HttpPost]
        [Route("refresh")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Refresh([FromBody] AuthResponseDetails request)
        {
            var authResponse = await _authManager.VerifyRefreshToken(request);
            if (authResponse == null)
            {
                return Unauthorized();
            }
            return Ok(authResponse);
        }

    }
}

