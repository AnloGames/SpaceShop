﻿using Microsoft.EntityFrameworkCore;
using SpaceShop_Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace SpaceShop.Data
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
    }
}
