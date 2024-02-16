using SyncronizationBot.Domain.Model.Database.Base;


namespace SyncronizationBot.Domain.Repository.Base
{
    public interface IRepository<T>: IReadCommandRepository<T>, 
                                     IWriteCommandRepository<T>
                           where T : Entity
    {
        
    }
}
