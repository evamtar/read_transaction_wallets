using MediatR;
using SyncronizationBot.Application.UpdateCommands.Base.Commands;
using SyncronizationBot.Application.UpdateCommands.Base.Response;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Repository.Base;

namespace SyncronizationBot.Application.UpdateCommands.Base.Handlers
{
    public class BaseUpdateCommandHandler<X, W, T> : IRequestHandler<X, W>
                                           where X : BaseUpdateCommand<W, T>
                                           where W : BaseUpdateCommandResponse<T>
                                           where T : Entity
    {
        private readonly IWriteCommandRepository<T> _repository;

        public BaseUpdateCommandHandler(IRepository<T> repository)
        {
            _repository = repository ?? throw new ArgumentException($"IRepository<T> --> {typeof(T)} is null here.");
        }

        public async Task<W> Handle(X request, CancellationToken cancellationToken)
        {
            if (request.Entity != null)
            {
                var entity = await _repository.Edit(request.Entity);
                var response = Activator.CreateInstance<W>();
                response.Entity = entity;
                return response;
            }
            throw new ArgumentException($"Entity --> {typeof(T)} is null here.");
        }
    }
}
