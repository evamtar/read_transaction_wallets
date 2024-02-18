using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Repository.SQLServer.Base;
using SyncronizationBot.Domain.Service.RecoveryService.Base;
using System;
using System.Linq.Expressions;

namespace SyncronizationBot.Service.InternalServices.Base
{
    public class ServiceBase<T> : IServiceBase<T> where T : Entity
    {
        private readonly IReadCommandRepository<T> _repository;
        public ServiceBase(IRepository<T> repository)
        {
            this._repository = repository;
        }

        public async Task<T?> FindFirstOrDefault(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null)
        {
            return await this._repository.FindFirstOrDefaultAsync(predicate, keySelector);
        }

        public async Task<T?> Get(Guid id)
        {
            return await this._repository.GetAsync(id);
        }

        public async Task<List<T>> Get(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null)
        {
            return await this._repository.GetAsync(predicate, keySelector);
        }

        public async Task<List<T>> GetAll()
        {
            return await this._repository.GetAllAsync();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
