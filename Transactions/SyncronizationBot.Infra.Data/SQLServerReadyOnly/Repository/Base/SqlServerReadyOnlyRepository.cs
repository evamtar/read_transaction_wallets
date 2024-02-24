using SyncronizationBot.Domain.Model.Database.Base;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SyncronizationBot.Domain.Repository.SQLServerReadyOnly.Base;
using SyncronizationBot.Infra.Data.SQLServerReadyOnly.Context;


namespace SyncronizationBot.Infra.Data.SQLServerReadyOnly.Repository.Base
{
    public class SqlServerReadyOnlyRepository<T> : ISqlServerReadyOnlyRepository<T> where T : Entity
    {
        private readonly SqlServerReadyOnlyContext _context;
        private DbSet<T> DbSet { get; set; }

        public SqlServerReadyOnlyRepository(SqlServerReadyOnlyContext context)
        {
            _context = context;
            this.DbSet = null!;
            this.DbSetEntity();
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
    }
}
