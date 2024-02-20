using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Linq.Expressions;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Repository.MongoDB.Base;
using SyncronizationBot.Infra.Data.MongoDB.Context;



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

        public void CreateColletcionAsync()
        {
            
            DropCollectionAsync();
            _database.CreateCollection(typeof(T).Name);
        }

        public void DropCollectionAsync()
        {
            if (_database.GetCollection<T>(typeof(T).Name) != null)
                _database.DropCollection(typeof(T).Name);
        }

        public void BulkWrite(List<T> listData) 
        {
            _database.GetCollection<T>(typeof(T).Name).BulkWrite(listData.Select(entity => new InsertOneModel<T>(entity)));
        }

        #endregion

        #region Add / Update / Delete Methods

        public List<T> AddRange(List<T> listItems) 
        {
            _context.Set<T>().AddRange(listItems);
            _context.SaveChanges();
            return listItems;
        }

        public T Add(T item)
        {
            _context.Set<T>().Add(item);
            _context.SaveChanges();
            return item;
        }

        public void DeleteById(Guid id)
        {
            var entity = GetAsync(id).GetAwaiter().GetResult();
            _context.Entry(entity).State = EntityState.Detached;
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public T Update(T item)
        {
            _context.Entry(item).State = EntityState.Detached;
            _context.Set<T>().Update(item);
            _context.SaveChanges();
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
