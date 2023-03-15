using System.ComponentModel.DataAnnotations;
//using Microsoft.Build.Framework;

namespace HotelListing.API.Models.Hotels.Users
{
    public class ApiUserDetails:Login
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
       
    }

}
