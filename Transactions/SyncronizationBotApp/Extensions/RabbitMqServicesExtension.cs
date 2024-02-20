using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SyncronizationBot.Domain.Service.RabbitMQ.AlertPriceQueue;
using SyncronizationBot.Domain.Service.RabbitMQ.LogMessageQueue;
using SyncronizationBot.Domain.Service.RabbitMQ.TokenAlhaQueue;
using SyncronizationBot.Domain.Service.RabbitMQ.TransactionsQueue;
using SyncronizationBot.Domain.Service.RabbitMQ.TrasanctionQueue;
using SyncronizationBot.Domain.Service.RabbitMQ.UpdateQueue;
using SyncronizationBot.Service.RabbitMQ.AlertPriceQueue.Configs;
using SyncronizationBot.Service.RabbitMQ.AlertPriceQueue;
using SyncronizationBot.Service.RabbitMQ.LogMessageQueue.Configs;
using SyncronizationBot.Service.RabbitMQ.LogMessageQueue;
using SyncronizationBot.Service.RabbitMQ.TokenAlhaQueue.Configs;
using SyncronizationBot.Service.RabbitMQ.TokenAlhaQueue;
using SyncronizationBot.Service.RabbitMQ.TransactionsQueue.Configs;
using SyncronizationBot.Service.RabbitMQ.TransactionsQueue;
using SyncronizationBot.Service.RabbitMQ.TrasanctionQueue.Configs;
using SyncronizationBot.Service.RabbitMQ.TrasanctionQueue;
using SyncronizationBot.Service.RabbitMQ.UpdateQueue.Configs;
using SyncronizationBot.Service.RabbitMQ.UpdateQueue;

namespace SyncronizationBotApp.Extensions
{
    public static class RabbitMqServicesExtension
    {
        public static IServiceCollection AddRabbitMqServices(this IServiceCollection services, IConfiguration configuration) 
        {
            services.Configure<AlertPriceQueueConfiguration>(configuration.GetSection("AlertPriceQueue"));
            services.Configure<LogMessageQueueConfig>(configuration.GetSection("LogMessageQueue"));
            services.Configure<TokenAlhaQueueConfig>(configuration.GetSection("TokenAlhaQueue"));
            services.Configure<TransactionsQueueConfig>(configuration.GetSection("TransactionsQueue"));
            services.Configure<TrasanctionQueueConfig>(configuration.GetSection("TrasanctionQueue"));
            services.Configure<UpdateQueueConfiguration>(configuration.GetSection("UpdateQueue"));

            services.AddScoped<IPublishAlertPriceService, PublishAlertPriceService>();
            services.AddScoped<IPublishLogService, PublishLogService>();
            services.AddScoped<IPublishTokenAlphaService, PublishTokenAlphaService>();
            services.AddScoped<IPublishTransactionsService, PublishTransactionsService>();
            services.AddScoped<IPublishTransactionService, PublishTransactionService>();
            services.AddScoped<IPublishUpdateService, PublishUpdateService>();
            return services;
        }
    }
}
