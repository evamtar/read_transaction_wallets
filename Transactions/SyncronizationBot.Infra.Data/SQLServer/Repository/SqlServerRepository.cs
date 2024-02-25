using SyncronizationBot.Domain.Model.Database.Base;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Infra.Data.SQLServer.Context;


namespace SyncronizationBot.Infra.Data.SQLServer.Repository
{
    public class SqlServerRepository<T> : IRepository<T> where T : Entity
    {
        private readonly SqlServerContext _context;
        private DbSet<T> DbSet { get; set; }

        public SqlServerRepository(SqlServerContext context)
        {
            _context = context;
            DbSet = null!;
            DbSetEntity();
        }

        public T Add(T item)
        {
            DbSet.Add(item);
            return item;
        }

        public async Task<T> AddAsync(T item)
        {
            await DbSet.AddAsync(item);
            return item;
        }

        public List<T> AddRange(List<T> items)
        {
            DbSet.AddRange(items);
            return items;
        }

        public async Task<List<T>> AddRangeAsync(List<T> items)
        {
            await DbSet.AddRangeAsync(items);
            return items;
        }

        public void DeleteById(Guid id)
        {
            var entity = GetAsync(id).GetAwaiter().GetResult();
            DbSet.Entry(entity).State = EntityState.Detached;
            DbSet.Remove(entity);
        }
        public async Task DeleteByIdAsync(Guid id)
        {
            var entity = await GetAsync(id);
            DbSet.Entry(entity).State = EntityState.Detached;
            DbSet.Remove(entity);
        }

        public void Delete(T entity)
        {
            DbSet.Entry(entity).State = EntityState.Detached;
            DbSet.Remove(entity);
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
            DbSet.Update(item);
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
                return await DbSet.AsNoTracking().Where(predicate).OrderBy(keySelector).ToListAsync();
            else
                return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<T?> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!)
        {
            if (keySelector != null)
                return await DbSet.AsNoTracking().Where(predicate).OrderBy(keySelector).FirstOrDefaultAsync();
            else
                return await DbSet.AsNoTracking().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await DbSet.AsNoTracking().ToListAsync();
        }

        #region Private Methods

        private void DbSetEntity()
        {
            DbSet = _context.Set<T>();
        }

        #endregion

        #region Not Implemented Methods

        public void CreateColletcionAsync() => throw new NotImplementedException();


        public void DropCollectionAsync() => throw new NotImplementedException();

        public void BulkWrite(List<T> listData) => throw new NotImplementedException();

        #endregion
    }
}
