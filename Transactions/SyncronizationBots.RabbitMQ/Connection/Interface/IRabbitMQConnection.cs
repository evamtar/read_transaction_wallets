using RabbitMQ.Client;

namespace SyncronizationBots.RabbitMQ.Connection.Interface
{
    public interface IRabbitMQConnection : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
