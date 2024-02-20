using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SyncronizationBots.RabbitMQ.Configuration;
using SyncronizationBots.RabbitMQ.Connection.Interface;
using SyncronizationBots.RabbitMQ.Connection;



namespace SyncronizationBots.RabbitMQ.Extension
{
    public static class RabbitMQExtension
    {
        public static void AddRabitMqConnection(this IServiceCollection services, IConfiguration configuration) 
        {
            services.Configure<RabbitMqConfiguration>(a => configuration.GetSection(nameof(RabbitMqConfiguration)).Bind(a));
            services.AddScoped<IRabbitMQConnection, RabbitMQConnection>();
        }
        
    }
}
