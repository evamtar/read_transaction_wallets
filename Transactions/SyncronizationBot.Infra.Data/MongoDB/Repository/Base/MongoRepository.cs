using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Linq.Expressions;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Repository.MongoDB.Base;
using SyncronizationBot.Infra.Data.MongoDB.Context;



namespace SyncronizationBot.Infra.Data.MongoDB.Repository.Base
{
    public class MongoRepository<T> : IMongoRepository<T> where T : Entity
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoClient _client;
        private readonly MongoDbContext _context;
        private DbSet<T> DbSet { get; set; }
        public MongoRepository(MongoDbContext context)
        {
            _context = context;
            this.DbSet = null!;
            this.DbSetEntity();
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
            DbSet.AddRange(listItems);
            return listItems;
        }

        public T Add(T item)
        {
            DbSet.Add(item);
            return item;
        }

        public void DeleteById(Guid id)
        {
            var entity = GetAsync(id).GetAwaiter().GetResult();
            DbSet.Entry(entity).State = EntityState.Detached;
            DbSet.Remove(entity);
        }

        public void Delete(T entity)
        {
            DbSet.Entry(entity).State = EntityState.Detached;
            DbSet.Remove(entity);
        }

        public T Update(T item)
        {
            DbSet.Entry(item).State = EntityState.Detached;
            DbSet.Update(item);
            return item;
        }

        public void SaveChanges() 
        {
            _context.SaveChanges();
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
                return await DbSet.Where(predicate).OrderBy(keySelector).ToListAsync();
            else
                return await DbSet.Where(predicate).ToListAsync();
        }

        public async Task<T?> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!)
        {
            if (keySelector != null)
                return await DbSet.Where(predicate).OrderBy(keySelector).FirstOrDefaultAsync();
            else
                return await DbSet.Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        #endregion

        #region Private Methods

        private void DbSetEntity() 
        {
            DbSet = _context.Set<T>();
        }

        #endregion
    }
}
