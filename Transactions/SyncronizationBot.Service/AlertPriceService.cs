using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.Send;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Service.Base;

namespace SyncronizationBot.Service
{
    public class AlertPriceService : BaseService
    {
        public AlertPriceService(IMediator mediator, 
                                 IRunTimeControllerRepository runTimeControllerRepository,
                                 ITypeOperationRepository typeOperationRepository,
                                 IOptions<SyncronizationBotConfig> syncronizationBotConfig) : base(mediator, runTimeControllerRepository, typeOperationRepository, ETypeService.Price, syncronizationBotConfig)
        {
        }

        protected override async Task DoExecute(CancellationToken cancellationToken)
        {
            await this._mediator.Send(new SendAlertPriceCommand { }, cancellationToken);
        }
        
    }
}
