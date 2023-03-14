using System.ComponentModel.DataAnnotations.Schema;
using HotelListing.API.Data;

namespace HotelListing.API.Models.Country
{
    public class GetCountryModel:BaseCountryDetails
    {
        public int Id { get; set; }
    }
    
}
