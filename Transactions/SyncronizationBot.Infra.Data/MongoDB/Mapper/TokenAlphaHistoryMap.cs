using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    internal class TokenAlphaHistoryMap : BaseMapper<TokenAlphaHistory>
    {
        public TokenAlphaHistoryMap() : base(EDatabase.Mongodb)
        {
        }

        
        
    }
}
