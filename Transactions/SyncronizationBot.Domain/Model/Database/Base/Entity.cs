using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using SyncronizationBot.Domain.Model.CustomAttributes;
using static SyncronizationBot.Domain.Model.CustomAttributes.DbMongoMapperAttribute;

namespace SyncronizationBot.Domain.Model.Database.Base
{
    public class Entity
    {
        [BsonId]
        [JsonIgnore]

        [DbSqlServerMapper(SqlServerTarget.Ignore)]
        [DbMongoMapper(MongoTarget.Key)]
        public ObjectId _id { get; set; } = ObjectId.GenerateNewId();

        [DbSqlServerMapper(SqlServerTarget.Key)]
        public virtual Guid? ID { get; set; }

        public string JsonSerialize() 
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Entity? JsonDeserializer(string value)
        {
            return JsonConvert.DeserializeObject<Entity>(value);
        }
    }
}
