using SyncronizationBot.Domain.Model.Database.Base;
using System.Linq.Expressions;

namespace SyncronizationBot.Domain.Repository.Base.Interfaces
{
    public interface IRepository<T> where T : Entity
    {

        #region Manage Collections For Mongo

        void CreateColletcionAsync();
        void DropCollectionAsync();

        #endregion

        #region Add / Update / Delete Methods

        void BulkWrite(List<T> listData);
        T Add(T item);
        List<T> AddRange(List<T> listItems);
        T Update(T item);
        void DeleteById(Guid id);
        void Delete(T entity);
       
        #endregion

        #region Rollback Methods For All Context

        Task<List<T>> GetAllAsync();
        Task<T?> GetAsync(Guid id);
        Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!);
        Task<T?> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!);

        #endregion

        #region Sql Server Methods  FOR Add / Delete Methods Async

        Task<List<T>> AddRangeAsync(List<T> items);
        Task<T> AddAsync(T item);
        Task DeleteByIdAsync(Guid id);

        #endregion
    }
}
