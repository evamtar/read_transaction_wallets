using SyncronizationBot.Domain.Model.Database.Base;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Infra.Data.SQLServer.Context;


namespace SyncronizationBot.Infra.Data.SQLServer.Repository.ReadyOnly
{
    public class SqlServerReadyOnlyRepository<T> : IRepository<T> where T : Entity
    {
        private readonly SqlServerReadyOnlyContext _context;
        private DbSet<T> DbSet { get; set; }

        public SqlServerReadyOnlyRepository(SqlServerReadyOnlyContext context)
        {
            _context = context;
            DbSet = null!;
            DbSetEntity();
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
        public T Add(T item) => throw new NotImplementedException();
        public List<T> AddRange(List<T> listItems) => throw new NotImplementedException();
        public T Update(T item) => throw new NotImplementedException();
        public void DeleteById(Guid id) => throw new NotImplementedException();
        public void Delete(T entity) => throw new NotImplementedException();
        public Task<List<T>> AddRangeAsync(List<T> items) => throw new NotImplementedException();
        public Task<T> AddAsync(T item) => throw new NotImplementedException();
        public Task DeleteByIdAsync(Guid id) => throw new NotImplementedException();

        #endregion
    }
}
