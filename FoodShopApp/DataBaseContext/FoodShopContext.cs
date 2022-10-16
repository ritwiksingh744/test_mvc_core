using FoodShopApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FoodShopApp.DataBaseContext
{
    public class FoodShopContext : IdentityDbContext<ApplicationUser>
    {
        public FoodShopContext(DbContextOptions<FoodShopContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Catagories { get; set; }
    }
}
