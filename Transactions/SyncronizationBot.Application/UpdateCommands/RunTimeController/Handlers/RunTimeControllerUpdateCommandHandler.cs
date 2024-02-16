using SyncronizationBot.Application.UpdateCommands.Base.Handlers;
using SyncronizationBot.Application.UpdateCommands.RunTimeController.Commands;
using SyncronizationBot.Application.UpdateCommands.RunTimeController.Response;
using SyncronizationBot.Domain.Repository;
using Entity = SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Application.UpdateCommands.RunTimeController.Handlers
{
    public class RunTimeControllerUpdateCommandHandler : BaseUpdateCommandHandler<RunTimeControllerUpdateCommand, RunTimeControllerUpdateCommandResponse, Entity.RunTimeController>
    {
        public RunTimeControllerUpdateCommandHandler(IRunTimeControllerRepository repository) : base(repository)
        {
        }
    }
}
