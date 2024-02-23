using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Repository.MongoDB.Base;
using SyncronizationBot.Domain.Repository.SQLServer.Base;
using SyncronizationBot.Domain.Service.InternalService.Base;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace SyncronizationBot.Service.InternalServices.Base
{
    public class CachedServiceBase<T> : ICachedServiceBase<T> where T : Entity
    {
        private static ConcurrentDictionary<string, bool> Pairs = new ConcurrentDictionary<string, bool>();
        private readonly IReadCommandRepository<T> _readRepository;
        private readonly ICachedRepository<T> _cachedRepository;
        public CachedServiceBase(IRepository<T> readRepository, ICachedRepository<T> cachedRepository)
        {
            this._readRepository = readRepository;
            this._cachedRepository = cachedRepository;
            //Esquentar cache
            var cached = this.GetAllAsync().GetAwaiter().GetResult();
        }

        #region Add Update Delete Methods

        public void AddRange(List<T> listItems)
        {
            this._cachedRepository.BulkWrite(listItems);
        }

        public T Add(T entity)
        {
            return this._cachedRepository.Add(entity);
        }

        public T Update(T entity)
        {
            return this._cachedRepository.Update(entity);
        }

        public void DeleteById(Guid id)
        {
            this._cachedRepository.DeleteById(id);
        }

        public void Delete(T entity)
        {
            this._cachedRepository.Delete(entity);
        }

        public void SaveChanges() 
        { 
            this._cachedRepository.SaveChanges();
        }

        #endregion

        #region Rollback cachedMethods

        public async Task<T?> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!)
        {
            return await _cachedRepository.FindFirstOrDefaultAsync(predicate, keySelector);
        }

        public async Task<T?> GetAsync(Guid id)
        {
            return await _cachedRepository.GetAsync(id);
        }

        public async Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!)
        {
            return await this._cachedRepository.GetAsync(predicate, keySelector);
        }

        public async Task<List<T>> GetAllAsync()
        {
            var listCached = await this._cachedRepository.GetAllAsync();
            if (!(listCached?.Any() ?? false)) 
            {
                if (!IsLoaded())
                {
                    var listRelational = await this._readRepository.GetAllAsync();
                    if (listRelational?.Count > 0)
                    {
                        this._cachedRepository.BulkWrite(listRelational);
                        listCached = listRelational;
                    }
                    Pairs.TryAdd(typeof(T).Name, true);
                }
            }
            return listCached ?? new List<T>();
        }

        #endregion

        #region Dispose Implemented

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Private methods

        private bool IsLoaded() 
        {
            if (!Pairs.ContainsKey(typeof(T).Name))
                return false;
            return Pairs[typeof(T).Name] == true;
        }

        #endregion
    }
}
