using MediatR;
using SyncronizationBot.Application.Response.MainCommands.Triggers;

namespace SyncronizationBot.Application.Commands.MainCommands.Triggers
{
    public class VerifyAddTokenAlphaCommand : IRequest<VerifyAddTokenAlphaCommandResponse>
    {
        public Guid? TokenId { get; set; }
        public Guid? WalletId { get; set; }
        public decimal ValueBuySol { get; set; }
        public decimal ValueBuyUSDC { get; set; }
        public decimal ValueBuyUSDT { get; set; }
        public string? Signature { get; set; }
        public decimal? MarketCap { get; set; }
        public decimal? Price { get; set; }
        public DateTime? LaunchDate { get; set; }
    }
}
