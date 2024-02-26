using Newtonsoft.Json;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.RabbitMQ;
using System;
using System.Globalization;

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
            decimal.TryParse(value, CultureInfo.InvariantCulture, out var decimalValue);
            return decimalValue;
        }

        public static decimal? ToDecimal(this string value, int? decimals)
        {
            decimal.TryParse(value, CultureInfo.InvariantCulture, out var decimalValue);
            return decimalValue / GetDivisor(decimals);
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

        private static long? GetDivisor(int? decimals)
        {
            if (decimals == null) return null!;
            string number = "1";
            for (int i = 0; i < decimals; i++)
                number += "0";
            return long.Parse(number);
        }
    }
}
