using MediatR;
using SyncronizationBot.Application.UpdateCommand.Base.Response;
using SyncronizationBot.Domain.Model.Database.Base;


namespace SyncronizationBot.Application.UpdateCommand.Base.Command
{
    public class BaseUpdateCommand<W, T> : IRequest<W>
                                 where W : BaseUpdateCommandResponse<T>
                                 where T : Entity
    {
        public T? Entity { get; set; }

    }
}
