﻿using E_commerce.Data.Repository.IRepository;
using E_commerce.Models;
using E_Commerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderHeader> OrderHeader { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    ID = 1,
                    Name = "Mobiles",
                    createdAt = System.DateTime.Now.ToString(),
                    updatedAt = System.DateTime.Now.ToString()
                });
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ID = 1,
                    Title = "Apple Iphone 14",
                    Price = 200000,
                    Description = "Apple Iphone 14, ........................",
                    Rating = 4,
                    createdAt = System.DateTime.Now.ToString(),
                    updatedAt = System.DateTime.Now.ToString(),
                    CategoryId = 1,
                    ImageUrl = "image",
                });
        }
    }
}
