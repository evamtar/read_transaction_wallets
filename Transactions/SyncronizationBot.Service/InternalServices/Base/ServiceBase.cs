using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Domain.Repository.UnitOfWork;
using SyncronizationBot.Domain.Service.RecoveryService.Base;
using System.Linq.Expressions;

namespace SyncronizationBot.Service.InternalServices.Base
{
    public class ServiceBase<T> : IServiceBase<T> where T : Entity
    {
        private readonly IUnitOfWorkSqlServer _unitOfWorkSqlServer;
        private readonly IRepository<T> _repository;
        public ServiceBase(IUnitOfWorkSqlServer unitOfWorkSqlServer)
        {
            this._unitOfWorkSqlServer = unitOfWorkSqlServer;
            this._repository = GetRepository() ?? throw new ArgumentNullException(nameof(_repository));
        }

        public async Task<T?> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null)
        {
            return await this._repository.FindFirstOrDefaultAsync(predicate, keySelector);
        }

        public async Task<T?> GetAsync(Guid id)
        {
            return await this._repository.GetAsync(id);
        }

        public async Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null)
        {
            return await this._repository.GetAsync(predicate, keySelector);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await this._repository.GetAllAsync();
        }

        public void Dispose()
        {
            try
            {
                _unitOfWorkSqlServer.Dispose();
            }
            finally 
            {
                GC.SuppressFinalize(this);
            }
        }

        #region Private Methods

        private IRepository<T>? GetRepository() 
        { 
            return (IRepository<T>?)this._unitOfWorkSqlServer.GetType()?.GetProperty(typeof(T).Name + "Repository")?.GetValue(this._unitOfWorkSqlServer);
        }

        #endregion
    }
}
