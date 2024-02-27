using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.UnitOfWork;
using SyncronizationBot.Domain.Service.InternalService.Telegram;
using SyncronizationBot.Service.InternalServices.Base;

namespace SyncronizationBot.Service.InternalServices.Telegram
{
    public class TelegramChannelService : CachedServiceBase<TelegramChannel>, ITelegramChannelService
    {
        public TelegramChannelService(IUnitOfWorkSqlServerReadyOnly unitOfWorkSqlServerReadyOnly, IUnitOfWorkMongo unitOfWorkMongo) : base(unitOfWorkSqlServerReadyOnly, unitOfWorkMongo)
        {
        }
    }
}
