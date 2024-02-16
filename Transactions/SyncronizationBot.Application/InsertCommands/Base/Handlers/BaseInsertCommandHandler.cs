using MediatR;
using SyncronizationBot.Application.InsertCommands.Base.Commands;
using SyncronizationBot.Application.InsertCommands.Base.Response;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Repository.Base;

namespace SyncronizationBot.Application.InsertCommands.Base.Handlers
{
    public class BaseInsertCommandHandler<T, W> : IRequestHandler<BaseInsertCommand<T, W>, BaseInsertCommandResponse<T>>
                                        where T : Entity
                                        where W : BaseInsertCommandResponse<T>
    {
        private readonly IWriteCommandRepository<T> _repository;

        public BaseInsertCommandHandler(IRepository<T> repository)
        {
            _repository = repository ?? throw new ArgumentException($"IRepository<T> --> {typeof(T)} is null here.");
        }

        public async Task<BaseInsertCommandResponse<T>> Handle(BaseInsertCommand<T, W> request, CancellationToken cancellationToken)
        {
            if (request.Entity != null)
            {
                var entity = await _repository.Add(request.Entity);
                return new BaseInsertCommandResponse<T> { Entity = entity };
            }
            throw new ArgumentException($"Entity --> {typeof(T)} is null here.");
        }
    }
}
