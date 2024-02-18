using SyncronizationBot.Domain.Model.Database.Base;


namespace SyncronizationBot.Domain.Repository.SQLServer.Base
{
    public interface IWriteCommandRepository<T> where T : Entity
    {
        Task<T> Add(T item);
        Task<T> DetachedItem(T item);
        Task<T> AddSingleItem(T item);
        Task<T> Edit(T item);
        Task Delete(Guid id);
        Task Delete(T entity);
        Task Truncate(string tableName);
    }
}
