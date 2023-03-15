using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.API.Data.Configurations
{
    public class HotelConfigurations : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(

                new Hotel
                {
                    Id = 1,
                    Name = "Mazzolona",
                    Address = "water tank",
                    CountryId = 1,
                    Rating = 4.5
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Meghana",
                    Address = "Jayanagar",
                    CountryId = 3,
                    Rating = 3.5
                }
            );
        }
    }
}
