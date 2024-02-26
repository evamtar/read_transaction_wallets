using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SyncronizationBot.Service.RabbitMQ.Queue.UpdateQueue;
using SyncronizationBot.Service.RabbitMQ.Queue.TokenAlhaQueue;
using SyncronizationBot.Service.RabbitMQ.Queue.TransactionsQueue;
using SyncronizationBot.Service.RabbitMQ.Queue.TrasanctionQueue;
using SyncronizationBot.Service.RabbitMQ.Queue.UpdateQueue.Configs;
using SyncronizationBot.Service.RabbitMQ.Queue.TokenAlhaQueue.Configs;
using SyncronizationBot.Service.RabbitMQ.Queue.TransactionsQueue.Configs;
using SyncronizationBot.Service.RabbitMQ.Queue.TrasanctionQueue.Configs;
using SyncronizationBot.Service.RabbitMQ.Queue.AlertPriceQueue;
using SyncronizationBot.Service.RabbitMQ.Queue.LogMessageQueue;
using SyncronizationBot.Service.RabbitMQ.Queue.AlertPriceQueue.Configs;
using SyncronizationBot.Service.RabbitMQ.Queue.LogMessageQueue.Configs;
using SyncronizationBot.Domain.Service.RabbitMQ.Queue.AlertPriceQueue;
using SyncronizationBot.Domain.Service.RabbitMQ.Queue.LogMessageQueue;
using SyncronizationBot.Domain.Service.RabbitMQ.Queue.TokenAlhaQueue;
using SyncronizationBot.Domain.Service.RabbitMQ.Queue.TransactionsQueue;
using SyncronizationBot.Domain.Service.RabbitMQ.Queue.TrasanctionQueue;
using SyncronizationBot.Domain.Service.RabbitMQ.Queue.UpdateQueue;
using SyncronizationBot.Service.RabbitMQ.Queue.TokenInfoQueue.Configs;
using SyncronizationBot.Domain.Service.RabbitMQ.Queue.TokenInfoQueue;
using SyncronizationBot.Service.RabbitMQ.Queue.TokenInfoQueue;

namespace SyncronizationBotApp.Extensions
{
    public static class RabbitMqServicesExtension
    {
        public static IServiceCollection AddRabbitMqServices(this IServiceCollection services, IConfiguration configuration) 
        {
            services.Configure<AlertPriceQueueConfiguration>(configuration.GetSection("AlertPriceQueue"));
            services.Configure<LogMessageQueueConfig>(configuration.GetSection("LogMessageQueue"));
            services.Configure<TokenInfoQueueConfiguration>(configuration.GetSection("TokenInfoQueue"));
            services.Configure<TokenAlhaQueueConfig>(configuration.GetSection("TokenAlhaQueue"));
            services.Configure<TransactionsQueueConfig>(configuration.GetSection("TransactionsQueue"));
            services.Configure<TrasanctionQueueConfig>(configuration.GetSection("TrasanctionQueue"));
            services.Configure<UpdateQueueConfiguration>(configuration.GetSection("UpdateQueue"));

            services.AddScoped<IPublishAlertPriceService, PublishAlertPriceService>();
            services.AddScoped<IPublishLogService, PublishLogService>();
            services.AddScoped<IPublishTokenAlphaService, PublishTokenAlphaService>();
            services.AddScoped<IPublishTokenInfoService, PublishTokenInfoService>();
            services.AddScoped<IPublishTransactionsService, PublishTransactionsService>();
            services.AddScoped<IPublishTransactionService, PublishTransactionService>();
            services.AddScoped<IPublishUpdateService, PublishUpdateService>();
            return services;
        }
    }
}
