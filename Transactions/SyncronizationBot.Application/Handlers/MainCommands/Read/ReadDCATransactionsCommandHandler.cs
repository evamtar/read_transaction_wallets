using MediatR;
using SyncronizationBot.Application.Commands.MainCommands.Read;
using SyncronizationBot.Application.Commands.SolanaFM;
using SyncronizationBot.Application.Response.MainCommands.Read;


namespace SyncronizationBot.Application.Handlers.MainCommands.Read
{
    public class ReadDCATransactionsCommandHandler : IRequestHandler<ReadDCATransactionsCommand, ReadDCATransactionsCommandResponse>
    {

        private readonly IMediator _mediator;

        public ReadDCATransactionsCommandHandler(IMediator mediator)
        {
            this._mediator = mediator;
        }
        public async Task<ReadDCATransactionsCommandResponse> Handle(ReadDCATransactionsCommand request, CancellationToken cancellationToken)
        {
            await _mediator.Send(new ReadDCATransactionCommand{ });
            //await _mediator.Send(new RecoverySaveTransactionsCommand { });
            return new ReadDCATransactionsCommandResponse {  };
        }
        
    }
}