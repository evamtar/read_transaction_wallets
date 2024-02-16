using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.Read;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Service.Base;


namespace SyncronizationBot.Service
{
    public class ReadTransactionWalletsService: BaseService
    {
        public ReadTransactionWalletsService(IMediator mediator,
                                             IRunTimeControllerRepository runTimeControllerRepository,
                                             ITypeOperationRepository typeOperationRepository,
                                             IOptions<SyncronizationBotConfig> syncronizationBotConfig) : base(mediator, runTimeControllerRepository, typeOperationRepository, ETypeService.Transaction, syncronizationBotConfig)
        {
            
        }

        protected override async Task DoExecute(CancellationToken stoppingToken)
        {
            var response = await this._mediator.Send(new ReadWalletsForTransactionCommand { IsContingecyTransaction = base.IsContingecyTransaction });
            if (response.HasWalletsWithBalanceLoad)
                base.EndTransactionsContingencySum(response.TotalValidTransactions);
        }
        
    }
}

