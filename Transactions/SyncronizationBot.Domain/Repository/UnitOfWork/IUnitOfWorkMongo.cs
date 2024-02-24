

namespace SyncronizationBot.Domain.Repository.UnitOfWork
{
    public interface IUnitOfWorkMongo : IDisposable
    { 
        Task SaveChangesAsync();
        void SaveChanges();
    }
}
