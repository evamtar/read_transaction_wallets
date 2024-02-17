//using MediatR;
//using Microsoft.Extensions.Options;
//using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
//using SyncronizationBot.Domain.Model.Configs;
//using SyncronizationBot.Domain.Model.Enum;
//using SyncronizationBot.Domain.Service.InternalService.Utils;
//using SyncronizationBot.Service.HostedServices.Base;



//namespace SyncronizationBot.Service.HostedServices
//{
//    public class TestService : BaseHostedService
//    {
//        protected override IOptions<SyncronizationBotConfig>? Options => throw new NotImplementedException();

//        protected override ETypeService? TypeService => ETypeService.NONE;
        
//        public TestService(IMediator mediator) : base(mediator)
//        {
//            Console.WriteLine("Iniciando o serviço de teste de alertas");
//        }

//        protected override async Task DoExecute(CancellationToken cancellationToken)
//        {
//            await _mediator.Send(new RecoverySaveNewsTokensCommand { }, cancellationToken);
//        }

//    }
//}
