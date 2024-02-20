
using RabbitMQ.Client;

namespace SyncronizationBots.RabbitMQ.Queue.Interface
{
    public interface IQueueConfiguration
    {
        string? Exchange { get; }
        string? QueueName { get; }
        bool? Durable { get; }
        bool? Exclusive { get; }
        bool? AutoDelete { get; }
        bool? Mandatory { get; }
        Dictionary<string, object>? Arguments { get; }
    }
}
