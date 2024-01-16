using MediatR;
using SyncronizationBot.Application.Commands.Base;
using SyncronizationBot.Application.Response;

namespace SyncronizationBot.Application.Commands
{
    public class RecoverySaveTransactionsCommand : SearchCommand, IRequest<RecoverySaveTransactionsCommandResponse>
    {
        public Guid? WalletId { get;set; }
        public string? WalletHash { get; set; }
        public int? IdClassification { get; set; }
    }
}
