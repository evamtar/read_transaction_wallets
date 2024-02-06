using MediatR;
using SyncronizationBot.Application.Response.MainCommands.Send;
using SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Application.Commands.MainCommands.Send
{
    public class SendTransactionAlertsCommand : IRequest<SendTransactionAlertsCommandResponse>
    {
        public Dictionary<string, object>? Parameters { get; set; }
        public Guid? EntityId { get; set; }
        public int? IdClassification { get; set; }
        public Guid? WalletId { get; set; }
        public Transactions? Transactions { get; set; }
        public string? TokenSendedHash { get; set; }
        public string? TokenReceivedHash { get; set; }
        public List<string>? TokensMapped { get; set; }

        public static Dictionary<string, object> GetParameters(object[]? args) 
        {
            return SendAlertMessageCommand.GetParameters(args);
        }


    }
}
