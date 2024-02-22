using Newtonsoft.Json;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.RabbitMQ;

namespace SyncronizationBot.Domain.Extensions
{
    public static class StringExtension
    {
        public static MessageEvent<T>? ToMessageEvent<T>(this string message) where T: Entity
        {
            return JsonConvert.DeserializeObject<MessageEvent<T>>(message);
        }
       
    }
}
