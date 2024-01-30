using MediatR;
using SyncronizationBot.Application.Response.MainCommands.AddUpdate;

namespace SyncronizationBot.Application.Commands.MainCommands.AddUpdate
{
    public class RecoveryAddUpdateBalanceItemCommand : IRequest<RecoveryAddUpdateBalanceItemCommandResponse>
    {
        public Guid? WalleId { get; set; }
        public Guid? TokenId { get; set; }
        public string? Signature { get; set; }
        public string? TokenHash { get; set; }
        public decimal? Quantity { get; set; }
    }

}
