using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.RabbitMQ;


namespace SyncronizationBots.RabbitMQ.Publisher.Interface
{
    public interface IPublishMessage
    {
        Task Publish<T>(MessageEvent<T> @event) where T : Entity;
    }
}
