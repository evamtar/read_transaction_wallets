using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.Delete;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Service.Base;

namespace SyncronizationBot.Service
{
    public class DeleteOldsMessagesLogService : BaseService
    {
        public DeleteOldsMessagesLogService(IMediator mediator,
                                            IRunTimeControllerRepository runTimeControllerRepository,
                                            ITypeOperationRepository typeOperationRepository,
                                            IOptions<SyncronizationBotConfig> syncronizationBotConfig) : base(mediator, runTimeControllerRepository, typeOperationRepository, ETypeService.DeleteOldMessages, syncronizationBotConfig)
        {
        }

        protected override async Task DoExecute(CancellationToken cancellationToken)
        {
            await this._mediator.Send(new DeleteTelegramMessageCommand { }, cancellationToken);
        }
    }
}
