using AutoMapper;
using HotelListing.API.Data;
using HotelListing.API.Models.Country;
using HotelListing.API.Models.Hotels;

namespace HotelListing.API.Configuratons
{
    public class MapperConfig:Profile
    {
        public MapperConfig()
        {
            CreateMap<Country, CreateCountryModel>().ReverseMap();
            CreateMap<Country, GetCountryModel>().ReverseMap();
            CreateMap<Country, CountryDetailsModel>().ReverseMap();
            CreateMap<Country,UpdateCountryModel>().ReverseMap();
            CreateMap<Hotel, HotelsModel>().ReverseMap();
        }
    }
}
