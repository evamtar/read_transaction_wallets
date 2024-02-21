using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class TokenMap : BaseMapper<Token>
    {
        public TokenMap() : base(EDatabase.Mongodb)
        {
        }

        protected override void PropertiesWithConversion(EntityTypeBuilder<Token> builder)
        {
            //builder.Property(t => t.Supply).HasConversion<string?>();
        }
        
    }
}
