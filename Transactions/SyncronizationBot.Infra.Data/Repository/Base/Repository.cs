using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Repository.Base;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;


namespace SyncronizationBot.Infra.Data.Repository.Base
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly DbContext _context;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        public Repository(DbContext context)
        {
            _context = context;
        }

        public async Task<T> Add(T item)
        {
            await _semaphore.WaitAsync();
            try
            {
                await _context.Set<T>().AddAsync(item);
                await _context.SaveChangesAsync();
                return item;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<T> DetachedItem(T item)
        {
            try 
            {
                _context.Entry(item).State = EntityState.Detached;
                await _context.SaveChangesAsync();
            }catch{ }
            return item;
        }

        public async Task<T> AddSingleItem(T item)
        {
            await _semaphore.WaitAsync();
            try
            {
                _context.Entry(item).State = EntityState.Detached;
                await _context.Set<T>().AddAsync(item);
                await _context.SaveChangesAsync();
                return item;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task Delete(Guid id)
        {
            await _semaphore.WaitAsync();
            try
            {
                var entity = await Get(id);
                _context.Entry(entity).State = EntityState.Detached;
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task Delete(T entity)
        {
            await _semaphore.WaitAsync();
            try
            {
                _context.Entry(entity).State = EntityState.Detached;
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task Truncate(string tableName)
        {
            await _semaphore.WaitAsync();
            try
            {
                try
                {
                    await _context.Database.ExecuteSqlRawAsync($" TRUNCATE TABLE {tableName}; ");
                }
                catch
                {
                    await _context.Database.ExecuteSqlRawAsync($" DELETE FROM {tableName}; ");
                }
                await _context.SaveChangesAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<T> Edit(T item)
        {
            await _semaphore.WaitAsync();
            try
            {
                _context.Entry(item).State = EntityState.Detached;
                _context.Set<T>().Update(item);
                await _context.SaveChangesAsync();
                return item;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public virtual async Task<T?> Get(Guid id)
        {
            await _semaphore.WaitAsync();
            try
            {
                return await this.FindFirstOrDefault(e => e.ID == id);
            }
            finally
            {
                _semaphore.Release();
            }
        }
        public virtual async Task<List<T>> Get(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (keySelector != null)
                    return await _context.Set<T>().Where(predicate).OrderBy(keySelector).ToListAsync();
                else
                    return await _context.Set<T>().Where(predicate).ToListAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public virtual async Task<T?> FindFirstOrDefault(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (keySelector != null)
                    return await _context.Set<T>().Where(predicate).OrderBy(keySelector).FirstOrDefaultAsync();
                else
                    return await _context.Set<T>().Where(predicate).FirstOrDefaultAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public virtual async Task<List<T>> GetAll()
        {
            await _semaphore.WaitAsync();
            try
            {
                return await _context.Set<T>().ToListAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
