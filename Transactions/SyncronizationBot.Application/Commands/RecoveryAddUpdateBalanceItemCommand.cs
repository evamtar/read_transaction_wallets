using MediatR;
using SyncronizationBot.Application.Response;

namespace SyncronizationBot.Application.Commands
{
    public class RecoveryAddUpdateBalanceItemCommand : IRequest<RecoveryAddUpdateBalanceItemCommandResponse>
    {
        public Guid? WalleId { get; set; }
        public Guid? TokenId { get; set; }
        public string? TokenHash { get; set; }
        public decimal? Quantity { get; set; }
    }
    
}
