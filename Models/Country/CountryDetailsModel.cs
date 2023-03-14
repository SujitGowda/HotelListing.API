using HotelListing.API.Models.Hotels;

namespace HotelListing.API.Models.Country
{
    public class CountryDetailsModel:BaseCountryDetails
    {
        public int Id { get; set; }
        public List<HotelsModel> Hotels { get; set; }

    }
    
}
