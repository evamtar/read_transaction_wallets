using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Repository.MongoDB.Base;
using SyncronizationBot.Domain.Repository.SQLServer.Base;
using SyncronizationBot.Domain.Service.InternalService.Base;
using System.Linq.Expressions;

namespace SyncronizationBot.Service.InternalServices.Base
{
    public class CachedServiceBase<T> : ICachedServiceBase<T> where T : Entity
    {
        private readonly IReadCommandRepository<T> _readRepository;
        private readonly ICachedRepository<T> _cachedRepository;
        public CachedServiceBase(IRepository<T> readRepository, ICachedRepository<T> cachedRepository)
        {
            this._readRepository = readRepository;
            this._cachedRepository = cachedRepository;
        }

        #region Add Update Delete Methods

        public Task<List<T>> AddRange(List<T> listItems)
        {
            throw new NotImplementedException();
        }

        public async Task<T> AddAsync(T entity)
        {
            return await this._cachedRepository.AddAsync(entity);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            return await this._cachedRepository.UpdateAsync(entity);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            await this._cachedRepository.DeleteByIdAsync(id);
        }

        public async Task DeleteAsync(T entity)
        {
            await this._cachedRepository.DeleteAsync(entity);
        }

        #endregion

        #region Rollback cachedMethods

        public async Task<T?> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null)
        {
            return await _cachedRepository.FindFirstOrDefaultAsync(predicate, keySelector);
        }

        public async Task<T?> GetAsync(Guid id)
        {
            return await _cachedRepository.GetAsync(id);
        }

        public async Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null)
        {
            return await this._cachedRepository.GetAsync(predicate, keySelector);
        }

        public async Task<List<T>> GetAllAsync()
        {
            await this._cachedRepository.CreateColletcionAsync();
            return await this._cachedRepository.GetAllAsync();
        }

        #endregion

        #region Dispose Implemented

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
