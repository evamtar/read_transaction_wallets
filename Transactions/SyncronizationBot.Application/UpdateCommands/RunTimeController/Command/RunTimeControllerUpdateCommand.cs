using SyncronizationBot.Application.UpdateCommand.Base.Command;
using SyncronizationBot.Application.UpdateCommand.RunTimeController.Response;
using Entity = SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Application.UpdateCommand.RunTimeController.Command
{
    public class RunTimeControllerUpdateCommand : BaseUpdateCommand<RunTimeControllerUpdateCommandResponse, Entity.RunTimeController>
    {
    }
}
