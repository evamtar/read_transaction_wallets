


namespace SyncronizationBot.Domain.Service.BatchMessageConsumer.Base
{
    public interface IBatchMessageConsumer
    {
        Task HandleMessage(string? message, CancellationToken cancellationToken);
    }
}
