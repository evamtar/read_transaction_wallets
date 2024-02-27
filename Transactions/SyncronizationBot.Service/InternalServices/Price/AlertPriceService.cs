using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.UnitOfWork;
using SyncronizationBot.Domain.Service.InternalService.Price;
using SyncronizationBot.Service.InternalServices.Base;


namespace SyncronizationBot.Service.InternalServices.Price
{
    public class AlertPriceService : CachedServiceBase<AlertPrice>, IAlertPriceService
    {
        public AlertPriceService(IUnitOfWorkSqlServerReadyOnly unitOfWorkSqlServerReadyOnly, IUnitOfWorkMongo unitOfWorkMongo) : base(unitOfWorkSqlServerReadyOnly, unitOfWorkMongo)
        {
        }
    }
}
