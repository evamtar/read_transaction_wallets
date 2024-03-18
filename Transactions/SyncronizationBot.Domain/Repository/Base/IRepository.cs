using SyncronizationBot.Domain.Model.Database.Base;
using System;
using System.Linq.Expressions;

namespace SyncronizationBot.Domain.Repository.Base
{
    public interface IRepository<T> where T : Entity
    {
        Task<List<T>> GetAll();
        Task<T?> Get(Guid id);
        Task<List<T>> Get(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!);
        Task<T?> FindFirstOrDefault(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!);
        Task<T> Add(T item);
        Task<T> DetachedItem(T item);
        Task<T> AddSingleItem(T item);
        T Edit(T item);
        Task Delete(Guid id);
        void Delete(T entity);
        Task Truncate(string tableName);
    }
}
