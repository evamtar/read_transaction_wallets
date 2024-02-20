


namespace SyncronizationBot.Domain.Service.BatchMessageConsumer.Base
{
    public interface IBatchMessageConsumer
    {
        string QueueName { get; }
        ushort PrefetchCount { get; set; }

        Task ProcessMessage(string message, CancellationToken cancellationToken);
    }
}
