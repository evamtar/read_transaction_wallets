using SyncronizationBot.Domain.Model.Database.Base;
using System.Linq.Expressions;

namespace SyncronizationBot.Domain.Service.InternalService.Base
{
    public interface ICachedServiceBase<T> : IDisposable where T : Entity
    {
        Task<List<T>> GetAll();
        Task<T?> Get(Guid id);
        Task<List<T>> Get(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!);
        Task<T?> FindFirstOrDefault(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!);
        Task<T> Add(T item);
        Task<T> DetachedItem(T item);
        Task<T> AddSingleItem(T item);
        Task<T> Edit(T item);
        Task Delete(Guid id);
        Task Delete(T entity);
        Task Truncate(string tableName);
    }
}
