using SyncronizationBot.Domain.Model.Database.Base;
using System.Linq.Expressions;

namespace SyncronizationBot.Domain.Repository.MongoDB.Base
{
    public interface ICachedRepository<T> where T : Entity
    {
        #region Manage Collections

        Task CreateColletcionAsync();
        Task DropCollectionAsync();

        #endregion

        #region Add / Update / Delete Methods

        Task BulkWrite(List<T> listData);
        Task<T> AddAsync(T item);
        Task<List<T>> AddRange(List<T> listItems);
        Task<T> UpdateAsync(T item);
        Task DeleteByIdAsync(Guid id);
        Task DeleteAsync(T entity);

        #endregion

        #region Rollback Methods

        Task<List<T>> GetAllAsync();
        Task<T?> GetAsync(Guid id);
        Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!);
        Task<T?> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!);

        #endregion
    }
}
