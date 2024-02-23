using MediatR;
using SyncronizationBot.Application.DeleteCommands.Base.Commands;
using SyncronizationBot.Application.DeleteCommands.Base.Response;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Repository.SQLServer.Base;

namespace SyncronizationBot.Application.DeleteCommands.Base.Handlers
{
    public class BaseDeleteCommandHandler<X, W, T> : IRequestHandler<X, W>
                                           where X : BaseDeleteCommand<W, T>
                                           where W : BaseDeleteCommandResponse
                                           where T : Entity
    {
        private readonly IWriteCommandRepository<T> _repository;

        public BaseDeleteCommandHandler(IRepository<T> repository)
        {
            _repository = repository ?? throw new ArgumentException($"IRepository<T> --> {typeof(T)} is null here.");
        }

        public async Task<W> Handle(X request, CancellationToken cancellationToken)
        {
            if (request.Entity != null)
            {
                _repository.Delete(request.Entity);
                var response = Activator.CreateInstance<W>();
                response.IsDeleted = true;
                return response;
            }
            throw new ArgumentException($"Entity --> {typeof(T)} is null here.");
        }
    }
}
