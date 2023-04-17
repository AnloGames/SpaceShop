using Microsoft.EntityFrameworkCore;
using SpaceShop_Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace SpaceShop_DataMigrations
{
    public class ApplicationDbContext:IdentityDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):
            base(options)
        {

        }
        public DbSet<Category> Category { get; set; }

        public DbSet<MyModel> MyModel { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<ConnectionProductMyModel> ConnectionProductMyModel { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<QueryHeader> QueryHeader { get; set; }
        public DbSet<QueryDetail> QueryDetail { get; set; }
        public DbSet<OrderHeader> OrderHeader { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
    }
}
