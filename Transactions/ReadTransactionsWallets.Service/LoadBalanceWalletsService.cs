using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ReadTransactionsWallets.Application.Commands;
using ReadTransactionsWallets.Domain.Model.Configs;


namespace ReadTransactionsWallets.Service
{
    public class LoadBalanceWalletsService : BackgroundService
    {
        private readonly IOptions<ReadTransactionWalletsConfig> _options;
        private readonly IMediator _mediator;
        public LoadBalanceWalletsService(IMediator mediator,
                                         IOptions<ReadTransactionWalletsConfig> options)
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
