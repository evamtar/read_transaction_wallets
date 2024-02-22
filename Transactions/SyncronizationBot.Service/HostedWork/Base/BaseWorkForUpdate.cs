using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.RabbitMQ;
using SyncronizationBot.Domain.Service.RabbitMQ.Queue.UpdateQueue;

namespace SyncronizationBot.Service.HostedWork.Base
{
    public class BaseWorkForUpdate
    {
        
        private readonly IPublishUpdateService _publishUpdateService;
        public BaseWorkForUpdate(IPublishUpdateService publishUpdateService)
        {
            this._publishUpdateService = publishUpdateService;
        }

        public async Task PublishMessage<T>(T entity, string? instruction) where T : Entity 
        {
            await this._publishUpdateService.Publish(new MessageEvent<T>
            {
                CreateDate = DateTime.Now,
                Entity = entity,
                EventName = nameof(T) + "_" + instruction,
                Parameters = null
            });
        }
    }
}
