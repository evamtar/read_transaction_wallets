using SyncronizationBot.Domain.Model.Database.Base;
using System;
using System.Linq.Expressions;

namespace SyncronizationBot.Domain.Repository.Base
{
    public interface IRepository<T> where T : Entity
    {
        Task<IEnumerable<T>> GetAll();
        Task<T?> Get(Guid id);
        Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate);
        Task<T?> FindFirstOrDefault(Expression<Func<T, bool>> predicate);
        Task<T> Add(T item);
        Task<T> DetachedItem(T item);
        Task<T> AddSingleItem(T item);
        Task<T> Edit(T item);
        Task Delete(Guid id);
        Task Delete(T entity);
        Task Truncate(string tableName);
    }
}
