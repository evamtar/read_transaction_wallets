using MediatR;
using SyncronizationBot.Application.Response.MainCommands.Send;
using SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Application.Commands.MainCommands.Send
{
    public class SendTransactionAlertsCommand : IRequest<SendTransactionAlertsCommandResponse>
    {
        public Dictionary<string, object>? Parameters { get; set; }
        public int? IdClassification { get; set; }
        public Guid? WalletId { get; set; }
        public string? WalletHash { get; set; }
        public Transactions? Transactions { get; set; }
        public DateTime? DateOfTransfer { get; set; }
        public string? TokenSendedSymbol { get; set; }
        public string? TokenSendedHash { get; set; }
        public string? TokenSendedPoolSymbol { get; set; }
        public string? TokenSendedPoolHash { get; set; }
        public string? TokenReceivedSymbol { get; set; }
        public string? TokenReceivedHash { get; set; }
        public string? TokenReceivedName { get; set; }
        public string? TokenReceivedMintAuthority { get; set; }
        public string? TokenReceivedFreezeAuthority { get; set; }
        public bool? TokenReceivedIsMutable { get; set; }
        public string? TokenReceivedPoolSymbol { get; set; }
        public string? TokenReceivedPoolHash { get; set; }
        public decimal? PercentModify { get; set; }
        public List<string>? TokensMapped { get; set; }
        
    }
}
