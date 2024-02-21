using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using SyncronizationBot.Domain.Model.CustomAttributes;

namespace SyncronizationBot.Domain.Model.Database.Base
{
    public class Entity
    {
        [BsonId]

        [DbMapper(MongoTarget.Key, SqlServerTarget.Ignore)]
        public ObjectId _id { get; set; } = ObjectId.GenerateNewId();

        [DbMapper(SqlServerTarget.Key)]
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
