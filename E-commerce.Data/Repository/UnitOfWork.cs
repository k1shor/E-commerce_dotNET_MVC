using E_commerce.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICategoryRepository Category { get; set; }
        public IProductRepository Product { get; set; }
        public IUserRepository User { get; set; }
        public IShoppingCartRepository ShoppingCart { get; set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(db);
            Product = new ProductRepository(db);
            User = new UserRepository(db);
            ShoppingCart = new ShoppingCartRepository(db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
