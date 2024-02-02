using MediatR;
using SyncronizationBot.Application.Response.Birdeye;
using SyncronizationBot.Domain.Model.Database;


namespace SyncronizationBot.Application.Commands.Birdeye
{
    public class RecoverySaveBalanceBirdeyeCommand : IRequest<RecoverySaveBalanceBirdeyeCommandResponse>
    {
        public Guid? WalletId { get; set; }
        public string? WalletHash { get; set; }
    }
}
