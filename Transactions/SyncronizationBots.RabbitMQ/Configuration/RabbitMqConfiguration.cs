

using Microsoft.Extensions.Configuration;

namespace SyncronizationBots.RabbitMQ.Configuration
{
    public class RabbitMqConfiguration
    {
        public string? HostName { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? VirtualHost { get; set; }
        public bool? AutomaticRecoveryEnabled { get; set; }
        public int? RequestedHeartbeat { get; set; }
    }
}
