using MediatR;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.SolnetRpc.Balance.Response;

namespace SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.SolnetRpc.Balance.Command
{
    public class SolnetBalanceReadCommand : IRequest<SolnetBalanceReadCommandResponse>
    {
        public string? WalletHash { get; set; }
        public bool? IgnoreAmountValueZero { get; set; }
    }
}
