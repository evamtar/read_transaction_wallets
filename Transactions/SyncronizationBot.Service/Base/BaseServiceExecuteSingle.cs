using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;

namespace SyncronizationBot.Service.Base
{
    public class BaseServiceExecuteSingle<TRequest, TResponse> : BaseService
                                where TRequest : IRequest<TResponse>
    {
        public BaseServiceExecuteSingle(IMediator mediator, IRunTimeControllerRepository runTimeControllerRepository, ITypeOperationRepository typeOperationRepository, ETypeService typeService, IOptions<SyncronizationBotConfig> syncronizationBotConfig) : base(mediator, runTimeControllerRepository, typeOperationRepository, typeService, syncronizationBotConfig)
        {
        }

        protected override async Task DoExecute(CancellationToken cancellationToken)
        {
            await this._mediator.Send(Activator.CreateInstance<TRequest>(), cancellationToken);
        }
    }
}
