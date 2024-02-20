using MediatR;
using SyncronizationBot.Application.Response.MainCommands.Send;
using SyncronizationBot.Domain.Model.Enum;

namespace SyncronizationBot.Application.Commands.MainCommands.Send
{
    public class SendAlertMessageCommand : IRequest<SendAlertMessageCommandResponse>
    {
        public Guid? TypeOperationId { get; set; }
        public Guid? EntityId { get; set; }
        public int? IdSubLevel { get; set; }
        public Dictionary<string, object>? Parameters { get; set; }

        #region Helpers

        public static Dictionary<string, object> GetParameters(object[]? args)
        {
            var parameters = new Dictionary<string, object>();
            if (args != null && args.Any())
            {
                foreach (var obj in args)
                {
                    if (obj != null)
                        parameters.Add(obj?.ToString() ?? string.Empty, obj!);
                }
            }
            return parameters;
        }

        #endregion
    }
}
