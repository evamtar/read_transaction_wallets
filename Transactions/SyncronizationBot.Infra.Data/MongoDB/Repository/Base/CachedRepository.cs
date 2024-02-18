using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Linq.Expressions;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Repository.MongoDB.Base;



namespace SyncronizationBot.Infra.Data.MongoDB.Repository.Base
{
    public class CachedRepository<T> : ICachedRepository<T> where T : Entity
    {
        private readonly IMongoDatabase _database;
        private readonly DbContext _context;
        public CachedRepository(DbContext context)
        {
            _context = context;
            if (_context.Database is IMongoDatabase)
                _database = (IMongoDatabase)_context.Database;
            else
                throw new ArgumentException("_context.Database not implements IMongoDatabase");
        }

        public async Task CreateColletcion()
        {
            await DropCollection();
            _database.CreateCollection(typeof(T).Name);
        }

        public Task DropCollection()
        {
            if (_database.GetCollection<T>(typeof(T).Name) != null)
                _database.DropCollection(typeof(T).Name);
            return Task.CompletedTask;
        }

        public async Task<T> Add(T item)
        {
            await _context.Set<T>().AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task Delete(Guid id)
        {
            var entity = await Get(id);
            _context.Entry(entity).State = EntityState.Detached;
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T> Edit(T item)
        {
            _context.Entry(item).State = EntityState.Detached;
            _context.Set<T>().Update(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<T?> Get(Guid id)
        {
            return await FindFirstOrDefault(e => e.ID == id);
        }

        public async Task<List<T>> Get(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!)
        {
            if (keySelector != null)
                return await _context.Set<T>().Where(predicate).OrderBy(keySelector).ToListAsync();
            else
                return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<T?> FindFirstOrDefault(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!)
        {
            if (keySelector != null)
                return await _context.Set<T>().Where(predicate).OrderBy(keySelector).FirstOrDefaultAsync();
            else
                return await _context.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }
    }
}
