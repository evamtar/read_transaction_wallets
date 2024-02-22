using SyncronizationBot.Domain.Model.CustomAttributes;
using SyncronizationBot.Domain.Model.Database.Base;


namespace SyncronizationBot.Domain.Model.Database
{
    public class TypeOperation : Entity
    {
        public string? Name { get; set; }
        public int? IdTypeOperation { get; set; }
        public int? IdSubLevel { get; set; }

        [DbMongoMapper(MongoTarget.Ignore)]
        public virtual List<Transactions>? Transactions { get; set; }

        [DbMongoMapper(MongoTarget.Ignore)]
        public virtual List<AlertConfiguration>? AlertConfigurations { get; set; }
    }
}
