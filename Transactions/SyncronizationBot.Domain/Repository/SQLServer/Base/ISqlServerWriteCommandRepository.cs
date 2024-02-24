using SyncronizationBot.Domain.Model.Database.Base;


namespace SyncronizationBot.Domain.Repository.SQLServer.Base
{
    public interface ISqlServerWriteCommandRepository<T> where T : Entity
    {
        Task<List<T>> AddRangeAsync(List<T> items);
        Task<T> AddAsync(T item);
        T Update(T item);
        Task DeleteByIdAsync(Guid id);
        void Delete(T entity);
        Task SaveChangesAsync();
        void SaveChanges();
    }
}
