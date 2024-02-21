using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;

namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class TypeOperationMap : BaseMapper<TypeOperation>
    {
        public TypeOperationMap() : base(EDatabase.Mongodb)
        {
        }

        protected override void IgnoreProperties(EntityTypeBuilder<TypeOperation> builder)
        {
            builder.Ignore(to => to.Transactions);
            builder.Ignore(to => to.AlertConfigurations);
        }
        

    }
}
