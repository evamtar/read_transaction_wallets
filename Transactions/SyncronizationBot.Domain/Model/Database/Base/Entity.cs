
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace SyncronizationBot.Domain.Model.Database.Base
{
    public class Entity
    {
        [BsonId]
        [BsonElement(Order = 0)]
        public Guid? ID { get; set; }

        public string JsonSerialize() 
        {
            return JsonConvert.SerializeObject(this);
        }

        public Entity? JsonDeserializer(string value)
        {
            return JsonConvert.DeserializeObject<Entity>(value);
        }
    }
}
