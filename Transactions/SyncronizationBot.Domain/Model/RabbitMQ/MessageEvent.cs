
using Newtonsoft.Json;

using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.RabbitMQ
{
    public class MessageEvent<T> where T : Entity
    {
        public string? EventName { get; set; }
        public DateTime? CreateDate { get; set; } = DateTime.Now;
        public T? Entity { get; set; }
        public object? Parameters { get; set; }

        public string JsonSerialize()
        {
            return JsonConvert.SerializeObject(this);

        }
    }
}