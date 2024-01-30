using SyncronizationBot.Domain.Model.CrossCutting.Jupiter.Prices.Response;

namespace SyncronizationBot.Application.Response.MainCommands.RecoverySave
{
    public class RecoveryPriceCommandResponse
    {
        public Dictionary<string, TokenData>? Data { get; set; }
        public decimal? TimeTaken { get; set; }
    }
}
