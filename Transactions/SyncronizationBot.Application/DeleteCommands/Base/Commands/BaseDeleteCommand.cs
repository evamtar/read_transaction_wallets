using MediatR;
using SyncronizationBot.Application.DeleteCommands.Base.Response;
using SyncronizationBot.Domain.Model.Database.Base;


namespace SyncronizationBot.Application.DeleteCommands.Base.Commands
{
    public class BaseDeleteCommand<T, W> : IRequest<W>
                                 where T : Entity
                                 where W : BaseDeleteCommandResponse<T>
    {
        public T? Entity { get; set; }

    }
}
