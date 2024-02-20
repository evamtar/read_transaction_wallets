using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SyncronizationBot.Domain.Service.BatchMessageConsumer.Base;

namespace SyncronizationBot.Service.BatchMessageConsumer.Base
{
    public class BaseBatchMessageConsumer<T> : BackgroundService  where T : IBatchMessageConsumer
    {
        #region Readonly Variables

        private readonly IServiceScope _scope;
        protected T? Worker { get; set; }

        #endregion

        public BaseBatchMessageConsumer(IServiceProvider serviceProvider)
        {
            this._scope = serviceProvider.CreateScope();
            this.Worker = this._scope.ServiceProvider.GetRequiredService<T>();
            this.Worker = default;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                _scope.Dispose();
            }
            finally
            {
            }
            return base.StopAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            /* Create Event to process message */
            if(this.Worker != null) 
               await this.Worker!.ProcessMessage(string.Empty, stoppingToken);
        }
    }
}
