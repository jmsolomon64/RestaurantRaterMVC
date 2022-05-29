using Microsoft.EntityFrameworkCore;

namespace RestaurantRaterMVC.Data
{
    public class RestaurantRaterDbContext : DbContext
    {
        public RestaurantRaterDbContext(DbContextOptions<RestaurantRaterDbContext> options) : base(options) { }

        public DbSet<Restaurant> Restaurants {get; set;}
        public DbSet<Rating> Ratings {get; set;}
    }
}