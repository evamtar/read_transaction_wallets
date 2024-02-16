using MediatR;
using SyncronizationBot.Application.DeleteCommands.Base.Response;
using SyncronizationBot.Domain.Model.Database.Base;


namespace SyncronizationBot.Application.DeleteCommands.Base.Commands
{
    public class BaseDeleteCommand<W, T> : IRequest<W>
                                 where W : BaseDeleteCommandResponse
                                 where T : Entity
    {
        public T? Entity { get; set; }

    }
}
