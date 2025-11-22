using NeonArcade.Server.Models.DTOs;
using System.Linq.Expressions;

namespace NeonArcade.Server.Repositories.Interfaces
{
    public interface IRepository<T> where  T: class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            params Expression<Func<T, object>>[] includes
        ); 
        Task<PagedResult<T>> GetPagedAsync(
          int pageNumber,
          int pageSize,
          Expression<Func<T, bool>>? filter = null,
          Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
          params Expression<Func<T, object>>[] includes
        );
        Task<bool> ExistsAsync(int id);
        Task<int> CountAsync();
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T?> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes);
        Task DeleteAsync(int id);
    }
}
