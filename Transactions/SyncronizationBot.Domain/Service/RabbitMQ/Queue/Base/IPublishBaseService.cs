using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.RabbitMQ;


namespace SyncronizationBot.Domain.Service.RabbitMQ.Queue.Base
{
    public interface IPublishBaseService
    {
        Task Publish<T>(MessageEvent<T> @event) where T : Entity;
    }
}
