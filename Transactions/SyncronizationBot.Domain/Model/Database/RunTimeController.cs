using MongoDB.Bson.Serialization.Attributes;
using SyncronizationBot.Domain.Model.CustomAttributes;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.Enum;

namespace SyncronizationBot.Domain.Model.Database
{
    public class RunTimeController : Entity
    {

        [DbSqlServerMapper(SqlServerTarget.Ignore)]
        [DbMongoMapper(MongoTarget.Ignore)]
        public override Guid? ID 
        { 
            get => base.ID; 
            set => base.ID = value; 
        }
        [DbSqlServerMapper(SqlServerTarget.Key)]
        public int? RuntimeId { get; set; }

        [DbSqlServerMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? ConfigurationTimer { get; set; }
        public ETypeService TypeService { get; set; }
        public bool? IsRunning { get; set; }
        public string? JobName { get; set; }
        public string? JobDescription { get; set; }
        public bool? IsActive { get; set; }
    }
}
