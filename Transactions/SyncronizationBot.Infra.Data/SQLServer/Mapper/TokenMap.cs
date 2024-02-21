using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.SQLServer.Mapper
{
    public class TokenMap : BaseMapper<Token>
    {
        public TokenMap() : base(EDatabase.SqlServer)
        {
        }

        protected override void PropertiesWithConversion(EntityTypeBuilder<Token> builder)
        {
            builder.Property(t => t.Supply).HasConversion<string?>();
        }
        
    }
}
