using SyncronizationBot.Application.UpdateCommand.Base.Handler;
using SyncronizationBot.Application.UpdateCommand.RunTimeController.Command;
using SyncronizationBot.Application.UpdateCommand.RunTimeController.Response;
using SyncronizationBot.Domain.Repository.UnitOfWork;
using Entity = SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Application.UpdateCommand.RunTimeController.Handler
{
    public class RunTimeControllerUpdateCommandHandler : BaseUpdateCommandHandler<RunTimeControllerUpdateCommand, RunTimeControllerUpdateCommandResponse, Entity.RunTimeController>
    {
        public RunTimeControllerUpdateCommandHandler(IUnitOfWorkSqlServer unitOfWorkSqlServer) : base(unitOfWorkSqlServer)
        {
        }
    }
}
