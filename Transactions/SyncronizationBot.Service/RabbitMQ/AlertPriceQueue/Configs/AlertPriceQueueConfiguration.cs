using SyncronizationBots.RabbitMQ.Queue.Interface;


namespace SyncronizationBot.Service.RabbitMQ.AlertPriceQueue.Configs
{
    public class AlertPriceQueueConfiguration : IQueueConfiguration
    {
        public string? Exchange { get; set; }
        public string? QueueName { get; set; }
        public bool? Durable { get; set; }
        public bool? Exclusive { get; set; }
        public bool? AutoDelete { get; set; }
        public bool? Mandatory { get; set; }
        public Dictionary<string, object>? Arguments { get; set; }
    }
}
