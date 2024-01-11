using MediatR;
using ReadTransactionsWallets.Application.Response;


namespace ReadTransactionsWallets.Application.Commands
{
    public class RecoverySaveTelegramChannel : IRequest<RecoverySaveTelegramChannelResponse>
    {
        public EChannel Channel { get; set; }
    }
}
