using System.ComponentModel.DataAnnotations;
//using Microsoft.Build.Framework;

namespace HotelListing.API.Models.Hotels.Users
{
    public class ApiUserDetails
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
       // [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "Your password is limited to {2} to {1} charecters", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
