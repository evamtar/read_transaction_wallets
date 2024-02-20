using SyncronizationBot.Domain.Model.Database.Base;


namespace SyncronizationBot.Domain.Repository.SQLServer.Base
{
    public interface IWriteCommandRepository<T> where T : Entity
    {
        Task<List<T>> AddRangeAsync(List<T> items);
        Task<T> AddAsync(T item);
        Task<T> DetachedItemAsync(T item);
        Task<T> AddSingleItemAsync(T item);
        T Update(T item);
        Task DeleteByIdAsync(Guid id);
        Task DeleteAsync(T entity);
        Task TruncateAsync(string tableName);
    }
}
