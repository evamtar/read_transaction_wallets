using MediatR;
using SyncronizationBot.Application.InsertCommand.Base.Command;
using SyncronizationBot.Application.InsertCommand.Base.Response;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Domain.Repository.UnitOfWork;

namespace SyncronizationBot.Application.InsertCommand.Base.Handler
{
    public class BaseInsertCommandHandler<X, W, T> : IRequestHandler<X, W>
                                           where X : BaseInsertCommand<W, T>
                                           where W : BaseInsertCommandResponse<T>
                                           where T : Entity
    {
        private readonly IUnitOfWorkSqlServer _unitOfWorkSqlServer;
        private readonly IRepository<T> _repository;

        public BaseInsertCommandHandler(IUnitOfWorkSqlServer unitOfWorkSqlServer)
        {
            _unitOfWorkSqlServer = unitOfWorkSqlServer ?? throw new ArgumentNullException($"IUnitOfWorkSqlServer --> {typeof(T)} is null here.");
            _repository = GetRepository() ?? throw new ArgumentNullException($"Repository --> {typeof(T)} is null here.");
        }

        public async Task<W> Handle(X request, CancellationToken cancellationToken)
        {
            if (request.Entity != null)
            {
                var entity = await _repository.AddAsync(request.Entity);
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
