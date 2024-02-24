using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Domain.Repository.UnitOfWork;
using SyncronizationBot.Domain.Service.InternalService.Base;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace SyncronizationBot.Service.InternalServices.Base
{
    public class CachedServiceBase<T> : ICachedServiceBase<T> where T : Entity
    {
        private static ConcurrentDictionary<string, bool> Pairs = new ConcurrentDictionary<string, bool>();
        private readonly IRepository<T> _sqlServerRepository;
        private readonly IRepository<T> _mongoRepository;
        private readonly IUnitOfWorkSqlServerReadyOnly _unitOfWorkSqlServerReadyOnly;
        private readonly IUnitOfWorkMongo _unitOfWorkMongo;
        public CachedServiceBase(IUnitOfWorkSqlServerReadyOnly unitOfWorkSqlServerReadyOnly, IUnitOfWorkMongo unitOfWorkMongo)
        {
            this._unitOfWorkSqlServerReadyOnly = unitOfWorkSqlServerReadyOnly;
            this._unitOfWorkMongo = unitOfWorkMongo;
            this._sqlServerRepository = GetReadyOnlyRepository() ?? throw new ArgumentNullException(nameof(_sqlServerRepository));
            this._mongoRepository = GetCachedRepository() ?? throw new ArgumentNullException(nameof(_mongoRepository));
            //Esquentar cache
            var cached = this.GetAllAsync().GetAwaiter().GetResult();
        }

        #region Add Update Delete Methods

        public void AddRange(List<T> listItems)
        {
            this.GetCachedRepository().BulkWrite(listItems);
        }

        public T Add(T entity)
        {
            return this._mongoRepository.Add(entity);
        }

        public T Update(T entity)
        {
            return this._mongoRepository.Update(entity);
        }

        public void DeleteById(Guid id)
        {
            this._mongoRepository.DeleteById(id);
        }

        public void Delete(T entity)
        {
            this._mongoRepository.Delete(entity);
        }

        public void SaveChanges() 
        { 
            this._unitOfWorkMongo.SaveChanges();
        }

        #endregion

        #region Rollback cachedMethods

        public async Task<T?> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!)
        {
            return await _mongoRepository.FindFirstOrDefaultAsync(predicate, keySelector);
        }

        public async Task<T?> GetAsync(Guid id)
        {
            return await _mongoRepository.GetAsync(id);
        }

        public async Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!)
        {
            return await this._mongoRepository.GetAsync(predicate, keySelector);
        }

        public async Task<List<T>> GetAllAsync()
        {
            var listCached = await this._mongoRepository.GetAllAsync();
            if (!(listCached?.Any() ?? false)) 
            {
                if (!IsLoaded())
                {
                    var listRelational = await this._sqlServerRepository.GetAllAsync();
                    if (listRelational?.Count > 0)
                    {
                        this._mongoRepository.BulkWrite(listRelational);
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
            try
            {
                this._unitOfWorkSqlServerReadyOnly.Dispose();
                this._unitOfWorkMongo.Dispose();
            }
            finally 
            {
                GC.SuppressFinalize(this);
            }
        }

        #endregion

        #region Private methods

        private IRepository<T>? GetReadyOnlyRepository()
        {
            return (IRepository<T>?)this._unitOfWorkSqlServerReadyOnly.GetType()?.GetProperty(typeof(T).Name + "Repository")?.GetValue(this._unitOfWorkSqlServerReadyOnly);
        }

        private IRepository<T>? GetCachedRepository()
        {
            return (IRepository<T>?)this._unitOfWorkMongo.GetType()?.GetProperty(typeof(T).Name + "Repository")?.GetValue(this._unitOfWorkMongo);
        }

        private bool IsLoaded() 
        {
            if (!Pairs.ContainsKey(typeof(T).Name))
                return false;
            return Pairs[typeof(T).Name] == true;
        }

        #endregion
        

    }
}
