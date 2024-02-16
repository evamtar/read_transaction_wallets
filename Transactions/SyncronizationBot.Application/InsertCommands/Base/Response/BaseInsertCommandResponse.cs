using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Application.InsertCommand.Base.Response
{
    public class BaseInsertCommandResponse<T> where T : Entity
    {
        public T? Entity { get; set; }
    }
}
