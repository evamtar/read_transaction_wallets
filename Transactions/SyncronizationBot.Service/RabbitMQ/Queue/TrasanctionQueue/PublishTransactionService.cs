﻿using Microsoft.Extensions.Options;
using SyncronizationBot.Domain.Service.RabbitMQ.Queue.TrasanctionQueue;
using SyncronizationBot.Service.RabbitMQ.Queue.TrasanctionQueue.Configs;
using SyncronizationBots.RabbitMQ.Connection.Interface;
using SyncronizationBots.RabbitMQ.Publisher;

namespace SyncronizationBot.Service.RabbitMQ.Queue.TrasanctionQueue
{
    public class PublishTransactionService : Publisher, IPublishTransactionService
    {
        public PublishTransactionService(IRabbitMQConnection rabbitMQConnection, IOptions<TrasanctionQueueConfig> configuration) : base(rabbitMQConnection, configuration.Value)
        {
        }
    }
}