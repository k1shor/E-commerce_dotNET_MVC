using E_commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
                    updatedAt = System.DateTime.Now.ToString()
                });
        }
    }
}
