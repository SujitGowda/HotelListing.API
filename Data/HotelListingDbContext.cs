using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Data
{
    public class HotelListingDbContext : DbContext
    {
        public HotelListingDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasData(
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
            modelBuilder.Entity<Hotel>().HasData(
                
                new Hotel
                {
                    Id = 1,
                    Name="Mazzolona",
                    Address="water tank",
                    CountryId=1,
                    Rating=4.5                    
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
