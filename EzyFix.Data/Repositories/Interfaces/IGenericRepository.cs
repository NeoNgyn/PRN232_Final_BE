using System.Linq.Expressions;
using EzyFix.DAL.Data.MetaDatas;
using Microsoft.EntityFrameworkCore.Query;

namespace EzyFix.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(object id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<int> SaveChangesAsync(); // Để commit transaction
    }
}
