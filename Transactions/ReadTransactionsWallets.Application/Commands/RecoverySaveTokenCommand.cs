using MediatR;
using ReadTransactionsWallets.Application.Response;

namespace ReadTransactionsWallets.Application.Commands
{
    public class RecoverySaveTokenCommand : IRequest<RecoverySaveTokenCommandResponse>
    {
        public string? TokenHash { get; set; }
    }
}
