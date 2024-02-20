using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace SyncronizationBot.Domain.Model.Database.Base
{
    public class Entity
    {
        [BsonId]

        public ObjectId CachedId { get; set; } = ObjectId.GenerateNewId();
        public Guid? ID { get; set; }

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
