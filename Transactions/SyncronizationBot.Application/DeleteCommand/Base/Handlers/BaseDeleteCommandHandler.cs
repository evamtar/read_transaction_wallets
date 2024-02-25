using MediatR;
using SyncronizationBot.Application.DeleteCommands.Base.Commands;
using SyncronizationBot.Application.DeleteCommands.Base.Response;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Domain.Repository.UnitOfWork;

namespace SyncronizationBot.Application.DeleteCommands.Base.Handlers
{
    public class BaseDeleteCommandHandler<X, W, T> : IRequestHandler<X, W>
                                           where X : BaseDeleteCommand<W, T>
                                           where W : BaseDeleteCommandResponse
                                           where T : Entity
    {
        private readonly IUnitOfWorkSqlServer _unitOfWorkSqlServer;
        private readonly IRepository<T> _repository;

        public BaseDeleteCommandHandler(IUnitOfWorkSqlServer unitOfWorkSqlServer)
        {
            _unitOfWorkSqlServer = unitOfWorkSqlServer ?? throw new ArgumentNullException($"IUnitOfWorkSqlServer --> {typeof(T)} is null here.");
            _repository = GetRepository() ?? throw new ArgumentNullException($"Repository --> {typeof(T)} is null here.");
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

        private IRepository<T>? GetRepository()
        {
            return (IRepository<T>?)this._unitOfWorkSqlServer.GetType()?.GetProperty(typeof(T).Name + "Repository")?.GetValue(this._unitOfWorkSqlServer);
        }
    }
}
