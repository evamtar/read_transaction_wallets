using MediatR;
using SyncronizationBot.Application.InsertCommands.Base.Response;
using SyncronizationBot.Domain.Model.Database.Base;


namespace SyncronizationBot.Application.InsertCommands.Base.Commands
{
    public class BaseInsertCommand<W, T> : IRequest<W>
                                 where W : BaseInsertCommandResponse<T>
                                 where T : Entity
    {
        public T? Entity { get; set; }

    }
}
