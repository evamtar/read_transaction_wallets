using SyncronizationBots.RabbitMQ.Queue.Interface;

namespace SyncronizationBot.Service.RabbitMQ.LogMessageQueue.Configs
{
    public class LogMessageQueueConfig : IQueueConfiguration
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
