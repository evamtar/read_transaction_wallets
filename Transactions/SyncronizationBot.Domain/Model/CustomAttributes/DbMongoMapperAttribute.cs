

using static SyncronizationBot.Domain.Model.CustomAttributes.DbMongoMapperAttribute;

namespace SyncronizationBot.Domain.Model.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DbMongoMapperAttribute : Attribute
    {
        private readonly MongoTarget _mongoTarget;

        public DbMongoMapperAttribute(MongoTarget mongoTarget)
        {
            _mongoTarget = mongoTarget;            
        }
        public MongoTarget GetMongoTarget() => _mongoTarget;
    }
    public enum MongoTarget
    {
        Ignore,
        Key
    }
}
