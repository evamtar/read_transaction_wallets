using MediatR;
using SyncronizationBot.Application.Commands.Base;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;
using SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Application.Commands.MainCommands.RecoverySave
{
    public class RecoverySaveTransactionsCommand : SearchCommand, IRequest<RecoverySaveTransactionsCommandResponse>
    {
        public Guid? WalletId { get; set; }
        public string? WalletHash { get; set; }
        public ClassWallet? ClassWallet { get; set; }
        public bool? IsContingecyTransaction { get; set; }
    }
}
