using SyncronizationBot.Domain.Model.Database.Base;
using System.Linq.Expressions;

namespace SyncronizationBot.Domain.Service.InternalService.Base
{
    public interface ICachedServiceBase<T> : IDisposable where T : Entity
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetAsync(Guid id);
        Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!);
        Task<T?> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!);
        Task<T> AddAsync(T item);
        Task<List<T>> AddRange(List<T> listItems);
        Task<T> UpdateAsync(T item);
        Task DeleteByIdAsync(Guid id);
        Task DeleteAsync(T entity);
    }
}
