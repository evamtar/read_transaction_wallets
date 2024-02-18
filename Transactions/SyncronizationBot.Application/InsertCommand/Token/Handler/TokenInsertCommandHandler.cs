using SyncronizationBot.Application.InsertCommand.Base.Handler;
using SyncronizationBot.Application.InsertCommand.Token.Command;
using SyncronizationBot.Application.InsertCommand.Token.Response;
using SyncronizationBot.Domain.Repository.SQLServer;
using Entity = SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Application.InsertCommand.Token.Handler
{
    public class TokenInsertCommandHandler : BaseInsertCommandHandler<TokenInsertCommand, TokenInsertCommandResponse, Entity.Token>
    {
        public TokenInsertCommandHandler(ITokenRepository repository) : base(repository)
        {
        }
    }
}
