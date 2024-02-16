using SyncronizationBot.Domain.Model.Database.Base;
using System.Linq.Expressions;

namespace SyncronizationBot.Domain.Service.RecoveryService.Base
{
    public interface IServiceBase<T> : IDisposable where T : Entity
    {
        Task<List<T>> GetAll();
        Task<T?> Get(Guid id);
        Task<List<T>> Get(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!);
        Task<T?> FindFirstOrDefault(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!);
    }
}
