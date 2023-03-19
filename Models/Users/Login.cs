using System.ComponentModel.DataAnnotations;
//using Microsoft.Build.Framework;

namespace HotelListing.API.Models.Users
{
    public class Login
    {
        [Required]
        // [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "Your password is limited to {2} to {1} charecters", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
