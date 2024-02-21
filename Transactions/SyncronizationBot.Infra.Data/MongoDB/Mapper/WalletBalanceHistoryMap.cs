using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;

namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class WalletBalanceHistoryMap : BaseMapper<WalletBalanceHistory>
    {
        public WalletBalanceHistoryMap() : base(EDatabase.Mongodb)
        {
        }
        
    }
}
