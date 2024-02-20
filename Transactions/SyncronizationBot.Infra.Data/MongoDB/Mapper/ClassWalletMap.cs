using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class ClassWalletMap : BaseMapper<ClassWallet>
    {
        public ClassWalletMap() : base(EDatabase.Mongodb)
        {
        }
    }
}
