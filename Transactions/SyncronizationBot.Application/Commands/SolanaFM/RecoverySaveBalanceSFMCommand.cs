using MediatR;
using SyncronizationBot.Application.Response.SolanaFM;


namespace SyncronizationBot.Application.Commands.SolanaFM
{
    public class RecoverySaveBalanceSFMCommand : IRequest<RecoverySaveBalanceSFMCommandResponse>
    {
        public Guid? WalletId { get; set; }
        public string? WalletHash { get; set; }
    }
}
