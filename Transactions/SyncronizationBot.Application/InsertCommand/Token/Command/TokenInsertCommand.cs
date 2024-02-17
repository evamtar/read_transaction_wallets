using SyncronizationBot.Application.InsertCommand.Base.Command;
using SyncronizationBot.Application.InsertCommand.Token.Response;
using Entity = SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Application.InsertCommand.Token.Command
{
    public class TokenInsertCommand : BaseInsertCommand<TokenInsertCommandResponse, Entity.Token>
    {
    }
}
