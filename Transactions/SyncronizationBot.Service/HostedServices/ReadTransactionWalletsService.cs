using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.Read;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Service.HostedServices.Base;


namespace SyncronizationBot.Service.HostedServices
{
    public class ReadTransactionWalletsService : BaseService
    {
        public ReadTransactionWalletsService(IMediator mediator,
                                             IRunTimeControllerRepository runTimeControllerRepository,
                                             ITypeOperationRepository typeOperationRepository,
                                             IOptions<SyncronizationBotConfig> syncronizationBotConfig) : base(mediator, runTimeControllerRepository, typeOperationRepository, ETypeService.Transaction, syncronizationBotConfig)
        {

        }

        protected override async Task DoExecute(CancellationToken stoppingToken)
        {
            var response = await _mediator.Send(new ReadWalletsForTransactionCommand { IsContingecyTransaction = IsContingecyTransaction });
            if (response.HasWalletsWithBalanceLoad)
                EndTransactionsContingencySum(response.TotalValidTransactions);
        }

    }
}

