
using MediatR;
using SyncronizationBot.Application.Response.MainCommands.AddUpdate;

namespace SyncronizationBot.Application.Commands.MainCommands.AddUpdate
{
    public class UpdateTokenAlphaCommand : IRequest<UpdateTokenAlphaCommandResponse>
    {
        public Guid? WalletId { get; set; }
        public Guid? TokenId { get; set; }
        public decimal? AmountTokenSell { get; set; }
        public decimal? AmountTokenSol { get; set; }
        public decimal? AmountTokenUSD { get; set; }
        public decimal? MarketCap { get; set; }
        public decimal? Price { get; set; }
    }
}
