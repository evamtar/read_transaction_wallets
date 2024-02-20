using SyncronizationBot.Domain.Model.Database.Base;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SyncronizationBot.Domain.Repository.SQLServer.Base;


namespace SyncronizationBot.Infra.Data.SQLServer.Repository.Base
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly DbContext _context;
        public Repository(DbContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T item)
        {
            await _context.Set<T>().AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<List<T>> AddRangeAsync(List<T> items)
        {
            await _context.Set<T>().AddRangeAsync(items);
            await _context.SaveChangesAsync();
            return items;
        }

        public async Task<T> DetachedItemAsync(T item)
        {
            try
            {
                _context.Entry(item).State = EntityState.Detached;
                await _context.SaveChangesAsync();
            }
            catch { }
            return item;
        }

        public async Task<T> AddSingleItemAsync(T item)
        {
            _context.Entry(item).State = EntityState.Detached;
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

        public async Task TruncateAsync(string tableName)
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

        public T Update(T item)
        {
            _context.Entry(item).State = EntityState.Detached;
            _context.Set<T>().Update(item);
            _context.SaveChanges();
            return item;
        }

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
    }
}
