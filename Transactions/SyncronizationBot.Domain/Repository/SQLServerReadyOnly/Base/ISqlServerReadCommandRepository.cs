﻿using SyncronizationBot.Domain.Model.Database.Base;
using System.Linq.Expressions;


namespace SyncronizationBot.Domain.Repository.SQLServerReadyOnly.Base
{
    public interface ISqlServerReadCommandRepository<T> where T : Entity
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetAsync(Guid id);
        Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!);
        Task<T?> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!);
    }
}