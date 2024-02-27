
using Newtonsoft.Json;

using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.RabbitMQ
{
    public class MessageEvent<T> where T : Entity
    {
        public string? EventName { get; set; }
        public DateTime? CreateDate { get; set; } = DateTime.Now;
        public T? Entity { get; set; }
        public Dictionary<string, object>? Parameters { get; set; }
        public Guid? EntityId { get; set; }
        public Guid? TypeOperationId { get; set; }
        public int? IdSubLevelAlertConfiguration { get; set; }

        public string JsonSerialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}