using Microsoft.EntityFrameworkCore;
using SpaceShop.Models;

namespace SpaceShop.Data
{
    public class ApplicationDbContext:DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):
            base(options)
        {

        }
        public DbSet<Category> Category { get; set; }

        public DbSet<MyModel> MyModel { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<ConnectionProductMyModel> ConnectionProductMyModel { get; set; }
    }
}
