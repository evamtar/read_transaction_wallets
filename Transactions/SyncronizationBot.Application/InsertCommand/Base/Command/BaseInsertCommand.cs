using MediatR;
using SyncronizationBot.Application.InsertCommand.Base.Response;
using SyncronizationBot.Domain.Model.Database.Base;


namespace SyncronizationBot.Application.InsertCommand.Base.Command
{
    public class BaseInsertCommand<W, T> : IRequest<W>
                                 where W : BaseInsertCommandResponse<T>
                                 where T : Entity
    {
        public T? Entity { get; set; }

    }
}
