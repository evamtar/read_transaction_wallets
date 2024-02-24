

using SyncronizationBot.Domain.Repository.Base.Interfaces;

namespace SyncronizationBot.Domain.Repository.UnitOfWork
{
    public interface IUnitOfWorkSqlServer : IDisposable
    {
        ITokenRepository TokenRepository { get; }
        IRunTimeControllerRepository RunTimeControllerRepository { get; }
        IWalletBalanceRepository WalletBalanceRepository { get; }
        IWalletBalanceHistoryRepository WalletBalanceHistoryRepository { get; }
        IWalletRepository WalletRepository { get; }
        
        Task SaveChangesAsync();
        void SaveChanges();
    }
}
