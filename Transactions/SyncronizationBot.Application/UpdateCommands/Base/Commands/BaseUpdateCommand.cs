using MediatR;
using SyncronizationBot.Application.UpdateCommands.Base.Response;
using SyncronizationBot.Domain.Model.Database.Base;


namespace SyncronizationBot.Application.UpdateCommands.Base.Commands
{
    public class BaseUpdateCommand<W, T> : IRequest<W>
                                 where W : BaseUpdateCommandResponse<T>
                                 where T : Entity
    {
        public T? Entity { get; set; }

    }
}
