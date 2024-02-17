//using MediatR;
//using Microsoft.Extensions.Options;
//using SyncronizationBot.Application.Commands.MainCommands.Read;
//using SyncronizationBot.Domain.Model.Configs;
//using SyncronizationBot.Domain.Model.Enum;
//using SyncronizationBot.Domain.Service.InternalService.Utils;
//using SyncronizationBot.Service.HostedServices.Base;


//namespace SyncronizationBot.Service.HostedServices
//{
//    public class ReadTransactionWalletsService : BaseHostedService
//    {
//        protected override IOptions<SyncronizationBotConfig>? Options { get { return this._syncronizationBotConfig; } }
//        protected override ETypeService? TypeService => ETypeService.Transaction;

//        private readonly IOptions<SyncronizationBotConfig> _syncronizationBotConfig;
//        public ReadTransactionWalletsService(IMediator mediator, 
//                                             IOptions<SyncronizationBotConfig> syncronizationBotConfig) : base(mediator)
//        {
//            this._syncronizationBotConfig = syncronizationBotConfig;
//        }

//        protected override async Task DoExecute(CancellationToken stoppingToken)
//        {
//            var response = await _mediator.Send(new ReadWalletsForTransactionCommand { IsContingecyTransaction = IsContingecyTransaction });
//            if (response.HasWalletsWithBalanceLoad)
//                EndTransactionsContingencySum(response.TotalValidTransactions);
//        }

//    }
//}

