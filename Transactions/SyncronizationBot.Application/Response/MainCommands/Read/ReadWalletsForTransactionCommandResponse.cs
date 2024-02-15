namespace SyncronizationBot.Application.Response.MainCommands.Read
{
    public class ReadWalletsForTransactionCommandResponse
    {
        public bool HasWalletsWithBalanceLoad { get; set; }
        public int? TotalValidTransactions { get; set; }
    }
}
