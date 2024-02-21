using SyncronizationBot.Domain.Model.CustomAttributes;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.Enum;

namespace SyncronizationBot.Domain.Model.Database
{
    public class RunTimeController : Entity
    {

        [DbMapper(MongoTarget.Ignore, SqlServerTarget.Ignore)]
        public override Guid? ID 
        { 
            get => base.ID; 
            set => base.ID = value; 
        }
        [DbMapper(SqlServerTarget.Key)]
        public int? RuntimeId { get; set; }

        [DbMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? ConfigurationTimer { get; set; }
        public ETypeService TypeService { get; set; }
        public bool? IsRunning { get; set; }
        public string? JobName { get; set; }
        public string? JobDescription { get; set; }
        public bool? IsActive { get; set; }
    }
}
