using CityInfoAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfoAPI.DbContexts
{
    public class CityInfoDbContext : DbContext
    {
        public CityInfoDbContext(DbContextOptions<CityInfoDbContext> options)
            : base(options)
        {

        }

        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
                .HasData(
                new City("Tehran")
                {
                    Id = 1,
                    Description = "tehran des",
                }, new City("Kerman")
                {
                    Id = 2,
                    Description = "kerman des",
                }, new City("Shiraz")
                {
                    Id = 3,
                    Description = "shiraz des",
                }
                );
            modelBuilder.Entity<PointOfInterest>()
                .HasData(
                new PointOfInterest("milad borj")
                {
                    Id = 1,
                    CityId = 1,
                    Description = "point 1 desc"
                }, new PointOfInterest("milad borj")
                {
                    Id = 2,
                    CityId = 1,
                    Description = "point 2 desc"
                }, new PointOfInterest("top khoneh")
                {
                    Id = 3,
                    CityId = 1,
                    Description = "point 3 desc"
                }
                );
            base.OnModelCreating(modelBuilder);
        }

    }
}
