using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Domain.Model.Configs;


namespace SyncronizationBot.Service
{
    public class LoadBalanceWalletsService : BackgroundService
    {
        private readonly IOptions<SyncronizationBotConfig> _options;
        private readonly IMediator _mediator;
        public LoadBalanceWalletsService(IMediator mediator,
                                         IOptions<SyncronizationBotConfig> options)
        {
            this._mediator = mediator;
            this._options = options;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken) 
        {
            Console.WriteLine("Iniciando o serviço de leitura de balanço nas wallets mapeadas");
            await this._mediator.Send(new ReadWalletsBalanceCommand { });
            Console.WriteLine("Finalizado");
            return;
        }
    }
}
