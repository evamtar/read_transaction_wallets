using MediatR;
using SyncronizationBot.Application.InsertCommands.Base.Response;
using SyncronizationBot.Domain.Model.Database.Base;


namespace SyncronizationBot.Application.InsertCommands.Base.Commands
{
    public class BaseInsertCommand<T, W> : IRequest<W>
                                 where T : Entity
                                 where W : BaseInsertCommandResponse<T>
    {
        public T? Entity { get; set; }

    }
}
