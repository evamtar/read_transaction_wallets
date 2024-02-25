using MediatR;
using SyncronizationBot.Application.UpdateCommand.Base.Command;
using SyncronizationBot.Application.UpdateCommand.Base.Response;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Domain.Repository.UnitOfWork;

namespace SyncronizationBot.Application.UpdateCommand.Base.Handler
{
    public class BaseUpdateCommandHandler<X, W, T> : IRequestHandler<X, W>
                                           where X : BaseUpdateCommand<W, T>
                                           where W : BaseUpdateCommandResponse<T>
                                           where T : Entity
    {
        private readonly IUnitOfWorkSqlServer _unitOfWorkSqlServer;
        private readonly IRepository<T> _repository;
        public BaseUpdateCommandHandler(IUnitOfWorkSqlServer unitOfWorkSqlServer)
        {
            _unitOfWorkSqlServer = unitOfWorkSqlServer ?? throw new ArgumentNullException($"IUnitOfWorkSqlServer --> {typeof(T)} is null here.");
            _repository = GetRepository() ?? throw new ArgumentNullException($"Repository --> {typeof(T)} is null here.");
        }

        public async Task<W> Handle(X request, CancellationToken cancellationToken)
        {
            if (request.Entity != null)
            {
                var entity = _repository.Update(request.Entity);
                var response = Activator.CreateInstance<W>();
                response.Entity = entity;
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
