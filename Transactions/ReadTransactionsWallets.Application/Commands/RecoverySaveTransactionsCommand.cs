using MediatR;
using ReadTransactionsWallets.Application.Commands.Base;
using ReadTransactionsWallets.Application.Response;

namespace ReadTransactionsWallets.Application.Commands
{
    public class RecoverySaveTransactionsCommand : SearchCommand, IRequest<RecoverySaveTransactionsCommandResponse>
    {
        public Guid? WalletId { get;set; }
        public string? WalletHash { get; set; }
        public int? IdClassification { get; set; }
    }
}
