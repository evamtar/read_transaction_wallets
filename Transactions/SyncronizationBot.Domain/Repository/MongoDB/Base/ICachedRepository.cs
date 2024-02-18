using SyncronizationBot.Domain.Model.Database.Base;
using System.Linq.Expressions;

namespace SyncronizationBot.Domain.Repository.MongoDB.Base
{
    public interface ICachedRepository<T> where T : Entity
    {
        Task CreateColletcion();
        Task DropCollection();
        Task<List<T>> GetAll();
        Task<T?> Get(Guid id);
        Task<List<T>> Get(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!);
        Task<T?> FindFirstOrDefault(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!);
        Task<T> Add(T item);
        Task<T> Edit(T item);
        Task Delete(Guid id);
        Task Delete(T entity);
    }
}
