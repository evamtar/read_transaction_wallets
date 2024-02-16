using MediatR;
using SyncronizationBot.Application.DeleteCommands.Base.Commands;
using SyncronizationBot.Application.DeleteCommands.Base.Response;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Repository.Base;

namespace SyncronizationBot.Application.DeleteCommands.Base.Handlers
{
    public class BaseDeleteCommandHandler<T, W> : IRequestHandler<BaseDeleteCommand<T, W>, BaseDeleteCommandResponse<T>>
                                        where T : Entity
                                        where W : BaseDeleteCommandResponse<T>
    {
        private readonly IWriteCommandRepository<T> _repository;

        public BaseDeleteCommandHandler(IRepository<T> repository)
        {
            _repository = repository ?? throw new ArgumentException($"IRepository<T> --> {typeof(T)} is null here.");
        }

        public async Task<BaseDeleteCommandResponse<T>> Handle(BaseDeleteCommand<T, W> request, CancellationToken cancellationToken)
        {
            if (request.Entity != null)
            {
                await _repository.Delete(request.Entity);
                return new BaseDeleteCommandResponse<T>{ };
            }
            throw new ArgumentException($"Entity --> {typeof(T)} is null here.");
        }
    }
}
