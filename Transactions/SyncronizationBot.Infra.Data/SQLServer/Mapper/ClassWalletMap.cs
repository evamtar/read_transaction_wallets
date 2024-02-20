using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.SQLServer.Mapper
{
    public class ClassWalletMap : BaseMapper<ClassWallet>
    {
        public ClassWalletMap() : base(EDatabase.SqlServer)
        {
        }
        
    }
}
