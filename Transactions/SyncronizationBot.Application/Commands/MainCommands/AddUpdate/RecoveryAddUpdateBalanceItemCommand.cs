using MediatR;
using SyncronizationBot.Application.Response.MainCommands.AddUpdate;
using SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Application.Commands.MainCommands.AddUpdate
{
    public class RecoveryAddUpdateBalanceItemCommand : IRequest<RecoveryAddUpdateBalanceItemCommandResponse>
    {
        public Transactions? Transactions { get; set; }
        public string? TokenSendedHash { get; set; }
        public string? TokenSendedPoolHash { get; set; }
        public string? TokenReceivedHash { get; set; }
        public string? TokenReceivedPoolHash { get; set; }
    }

}
