using Newtonsoft.Json;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.RabbitMQ;

namespace SyncronizationBot.Domain.Extensions
{
    public static class ObjectsExtension
    {
        #region String

        public static MessageEvent<T>? ToMessageEvent<T>(this string message) where T: Entity
        {
            return JsonConvert.DeserializeObject<MessageEvent<T>>(message);
        }

        public static decimal? ToDecimal(this string value) 
        { 
            decimal.TryParse(value, out var decimalValue);
            return decimalValue;
        }

        #endregion

        #region Double

        public static decimal? ToDecimal(this double value) 
        {
            return (decimal?)value;
        }

        #endregion

        #region Long

        public static decimal? ToDecimal(this long value) 
        {
            return value;
        }

        #endregion
    }
}
