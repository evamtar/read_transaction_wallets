using MediatR;
using SyncronizationBot.Application.Commands.Base;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;
using SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Application.Commands.MainCommands.RecoverySave
{
    public class RecoverySaveTransactionsOldForMappingCommand : SearchCommand, IRequest<RecoverySaveTransactionsOldForMappingCommandResponse>
    {
        public Guid? WalletId { get; set; }
        public string? WalletHash { get; set; }
        public ClassWallet? ClassWallet { get; set; }
        public bool? IsContingecyTransactions { get; set; }
    }
}
