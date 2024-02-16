using SyncronizationBot.Application.UpdateCommands.Base.Commands;
using SyncronizationBot.Application.UpdateCommands.RunTimeController.Response;
using Entity = SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Application.UpdateCommands.RunTimeController.Commands
{
    public class RunTimeControllerUpdateCommand : BaseUpdateCommand<RunTimeControllerUpdateCommandResponse, Entity.RunTimeController>
    {
    }
}
