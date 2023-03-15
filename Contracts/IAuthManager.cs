using HotelListing.API.Models.Hotels.Users;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Contracts
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(ApiUserDetails apiUserDtails);
        Task<AuthResponseDetails> Login(Login login);
    }
}
