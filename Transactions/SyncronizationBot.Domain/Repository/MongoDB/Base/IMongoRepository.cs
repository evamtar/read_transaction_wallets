using SyncronizationBot.Domain.Model.Database.Base;
using System.Linq.Expressions;

namespace SyncronizationBot.Domain.Repository.MongoDB.Base
{
    public interface IMongoRepository<T> where T : Entity
                                                   
    {
        #region Manage Collections

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
        void SaveChanges();

        #endregion

        #region Rollback Methods

        Task<List<T>> GetAllAsync();
        Task<T?> GetAsync(Guid id);
        Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!);
        Task<T?> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!);
        
        #endregion
    }
}
