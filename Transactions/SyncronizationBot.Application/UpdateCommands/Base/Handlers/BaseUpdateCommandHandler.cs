using MediatR;
using SyncronizationBot.Application.UpdateCommands.Base.Commands;
using SyncronizationBot.Application.UpdateCommands.Base.Response;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Repository.Base;

namespace SyncronizationBot.Application.UpdateCommands.Base.Handlers
{
    public class BaseUpdateCommandHandler<T, W> : IRequestHandler<BaseUpdateCommand<T, W>, BaseUpdateCommandResponse<T>>
                                        where T : Entity
                                        where W : BaseUpdateCommandResponse<T>
    {
        private readonly IWriteCommandRepository<T> _repository;

        public BaseUpdateCommandHandler(IRepository<T> repository)
        {
            _repository = repository ?? throw new ArgumentException($"IRepository<T> --> {typeof(T)} is null here.");
        }

        public async Task<BaseUpdateCommandResponse<T>> Handle(BaseUpdateCommand<T, W> request, CancellationToken cancellationToken)
        {
            if (request.Entity != null)
            {
                var entity = await _repository.Edit(request.Entity);
                return new BaseUpdateCommandResponse<T> { Entity = entity };
            }
            throw new ArgumentException($"Entity --> {typeof(T)} is null here.");
        }
    }
}
