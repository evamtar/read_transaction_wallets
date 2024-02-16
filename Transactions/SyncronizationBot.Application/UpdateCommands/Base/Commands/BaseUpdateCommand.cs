using MediatR;
using SyncronizationBot.Application.UpdateCommands.Base.Response;
using SyncronizationBot.Domain.Model.Database.Base;


namespace SyncronizationBot.Application.UpdateCommands.Base.Commands
{
    public class BaseUpdateCommand<T, W> : IRequest<W>
                                 where T : Entity
                                 where W : BaseUpdateCommandResponse<T>
    {
        public T? Entity { get; set; }

    }
}
