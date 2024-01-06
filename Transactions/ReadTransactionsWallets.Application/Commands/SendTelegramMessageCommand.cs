using MediatR;
using ReadTransactionsWallets.Application.Response;

namespace ReadTransactionsWallets.Application.Commands
{
    public class SendTelegramMessageCommand : IRequest<SendTelegramMessageCommandResponse>
    {
        public string? Message { get; set; }
        public EChannel Channel { get; set; }
    }

    public enum EChannel 
    {
        CallSolanaLog = 1,
        CallSolana = 2
    }
}
