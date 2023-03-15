using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.API.Data.Configurations
{
    public class CountryConfigurations : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
                new Country
                {
                    Id = 1,
                    Name = "India",
                    ShortName = "Ind"
                },
                new Country
                {
                    Id = 2,
                    Name = "Dubai",
                    ShortName = "UAE"
                },
                new Country
                {
                    Id = 3,
                    Name = "SriLanka",
                    ShortName = "SL"
                }
            );
        }
    }
}
