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
        T Add(T item);
        void AddRange(List<T> listItems);
        T Update(T item);
        void DeleteById(Guid id);
        void Delete(T entity);
        void SaveChanges();
    }
}
