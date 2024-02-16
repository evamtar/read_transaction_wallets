using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Application.UpdateCommands.Base.Response
{
    public class BaseUpdateCommandResponse<T> where T : Entity
    {
        public T? Entity { get; set; }
    }
}
