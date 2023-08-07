using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Data.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> FindAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        T FirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
        void Create(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);

    }
}
