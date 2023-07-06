using E_commerce.Data.Repository.IRepository;
using E_commerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Data.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Product Product)
        {
            /*            _db.Products.Update(Product);*/
            Product productToUpdate = _db.Products.FirstOrDefault(u => u.ID == Product.ID);
            if (Product.ImageUrl == null)
            {
                productToUpdate.Title = Product.Title;
                productToUpdate.Price = Product.Price;
                productToUpdate.Description = Product.Description;
                productToUpdate.Category = Product.Category;
                productToUpdate.CategoryId = Product.CategoryId;
                productToUpdate.Count_In_Stock = Product.Count_In_Stock;
                /*productToUpdate.ImageUrl = productToUpdate.ImageUrl;*/
            }
            else
            {
                productToUpdate.Title = Product.Title;
                productToUpdate.Price = Product.Price;
                productToUpdate.Description = Product.Description;
                productToUpdate.Category = Product.Category;
                productToUpdate.CategoryId = Product.CategoryId;
                productToUpdate.Count_In_Stock = Product.Count_In_Stock;
                productToUpdate.ImageUrl = Product.ImageUrl;
            }
        }
    }
}
