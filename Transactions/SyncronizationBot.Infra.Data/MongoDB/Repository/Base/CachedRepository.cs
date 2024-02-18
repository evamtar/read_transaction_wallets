using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Linq.Expressions;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Repository.MongoDB.Base;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Infra.Data.MongoDB.Context;
using System.Numerics;
using MongoDB.EntityFrameworkCore.Storage;




namespace SyncronizationBot.Infra.Data.MongoDB.Repository.Base
{
    public class CachedRepository<T> : ICachedRepository<T> where T : Entity
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoClient _client;
        private readonly MongoDbContext _context;
        public CachedRepository(MongoDbContext context)
        {
            _context = context;
            _client = new MongoClient(_context._connectionString);
            _database = _client.GetDatabase(_context._databaseName);
        }

        #region Manage Collections

        public async Task CreateColletcionAsync()
        {
            
            await DropCollectionAsync();
            _database.CreateCollection(typeof(T).Name);
        }

        public Task DropCollectionAsync()
        {
            if (_database.GetCollection<T>(typeof(T).Name) != null)
                _database.DropCollection(typeof(T).Name);
            return Task.CompletedTask;
        }

        public Task BulkWrite(List<T> listData) 
        {
            _database.GetCollection<T>(typeof(T).Name).BulkWrite(listData.Select(entity => new InsertOneModel<T>(entity)));
            return Task.CompletedTask;
        }

        #endregion

        #region Add / Update / Delete Methods

        public async Task<List<T>> AddRange(List<T> listItems) 
        {
            await _context.Set<T>().AddRangeAsync(listItems);
            await _context.SaveChangesAsync();
            return listItems;
        }

        public async Task<T> AddAsync(T item)
        {
            await _context.Set<T>().AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var entity = await GetAsync(id);
            _context.Entry(entity).State = EntityState.Detached;
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T> UpdateAsync(T item)
        {
            _context.Entry(item).State = EntityState.Detached;
            _context.Set<T>().Update(item);
            await _context.SaveChangesAsync();
            return item;
        }

        #endregion

        #region Rollback Methods



        public async Task<T?> GetAsync(Guid id)
        {
            return await FindFirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!)
        {
            if (keySelector != null)
                return await _context.Set<T>().Where(predicate).OrderBy(keySelector).ToListAsync();
            else
                return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<T?> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!)
        {
            if (keySelector != null)
                return await _context.Set<T>().Where(predicate).OrderBy(keySelector).FirstOrDefaultAsync();
            else
                return await _context.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        #endregion
    }
}
