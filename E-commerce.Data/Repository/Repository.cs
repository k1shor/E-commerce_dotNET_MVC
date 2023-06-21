using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using E_commerce.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        private DbSet<T> dbset;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbset = _db.Set<T>();
        }
        public void Create(T entity)
        {
            dbset.Add(entity);
        }

        public void Delete(T entity)
        {
            dbset.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            dbset.RemoveRange(entities);
        }

        public IEnumerable<T> FindAll()
        {
            IQueryable<T> query = dbset;
            return query.ToList();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbset.Where(filter);
            return query.FirstOrDefault();
        }

        
    }
}
