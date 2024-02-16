using SyncronizationBot.Domain.Model.Database.Base;
using System.Linq.Expressions;


namespace SyncronizationBot.Domain.Repository.Base
{
    public interface IReadCommandRepository<T> where T : Entity
    {
        Task<List<T>> GetAll();
        Task<T?> Get(Guid id);
        Task<List<T>> Get(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!);
        Task<T?> FindFirstOrDefault(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!);
    }
}
