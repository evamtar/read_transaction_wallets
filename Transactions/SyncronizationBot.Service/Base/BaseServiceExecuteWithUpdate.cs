using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;


namespace SyncronizationBot.Service.Base
{
    public class BaseServiceExecuteWithUpdate<TRequest, TResponse, WRequest, WResponse> : BaseServiceExecuteSingle<TRequest, TResponse>
                                where TRequest : IRequest<TResponse>
                                where WRequest : IRequest<WResponse>
    {

        public BaseServiceExecuteWithUpdate(IMediator mediator, IRunTimeControllerRepository runTimeControllerRepository, ITypeOperationRepository typeOperationRepository, ETypeService typeService, IOptions<SyncronizationBotConfig> syncronizationBotConfig) : base(mediator, runTimeControllerRepository, typeOperationRepository, typeService, syncronizationBotConfig)
        {
        }

        protected override async Task DoExecute(CancellationToken cancellationToken)
        {
            await base.DoExecute(cancellationToken);
            //await this._mediator.Send(Activator.CreateInstance<WRequest>(), cancellationToken);
        }
    }
}
