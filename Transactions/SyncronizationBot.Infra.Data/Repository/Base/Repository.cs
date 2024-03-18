using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Repository.Base;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;


namespace SyncronizationBot.Infra.Data.Repository.Base
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly DbContext _context;
        public Repository(DbContext context)
        {
            _context = context;
        }

        public async Task<T> Add(T item)
        {
            await _context.Set<T>().AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<T> DetachedItem(T item)
        {
            try 
            {
                _context.Entry(item).State = EntityState.Detached;
            }catch{ }
            return item;
        }

        public async Task<T> AddSingleItem(T item)
        {
            _context.Entry(item).State = EntityState.Detached;
            await _context.Set<T>().AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task SaveChangesASync() 
        {
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var entity = await Get(id);
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


        public async Task Truncate(string tableName)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync($" TRUNCATE TABLE {tableName}; ");
            }
            catch
            {
                await _context.Database.ExecuteSqlRawAsync($" DELETE FROM {tableName}; ");
            }
        }

        public T Edit(T item)
        {
            _context.Entry(item).State = EntityState.Detached;
            _context.Set<T>().Update(item);
            _context.SaveChanges();
            return item;
        }

        public virtual async Task<T?> Get(Guid id)
        {
            return await this.FindFirstOrDefault(e => e.ID == id);
        }
        public virtual async Task<List<T>> Get(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!)
        {
            if (keySelector != null)
                return await _context.Set<T>().Where(predicate).OrderBy(keySelector).ToListAsync();
            else
                return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public virtual async Task<T?> FindFirstOrDefault(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!)
        {
            if (keySelector != null)
                return await _context.Set<T>().Where(predicate).OrderBy(keySelector).FirstOrDefaultAsync();
            else
                return await _context.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }


        public virtual async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }
    }
}
