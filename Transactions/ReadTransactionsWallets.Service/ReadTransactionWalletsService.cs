using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ReadTransactionsWallets.Domain.Model.Configs;

namespace ReadTransactionsWallets.Service
{
    public class ReadTransactionWalletsService: BackgroundService
    {
        private readonly IOptions<ReadTransactionWalletsConfig> _options;
        public ReadTransactionWalletsService(IOptions<ReadTransactionWalletsConfig> options)
        {
            _options = options;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Iniciando o bot de leitura de transações efetuadas nas wallets mapeadas");
            using var timer = new PeriodicTimer(TimeSpan.FromMinutes(this._options.Value.ConfigurationTimer ?? 5));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                Console.WriteLine("Iniciando leitura" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                //Todo CODE

                Console.WriteLine("Finalizando leitura" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            }
            Console.WriteLine("Finalizado");
            return;
        }
    }
}
