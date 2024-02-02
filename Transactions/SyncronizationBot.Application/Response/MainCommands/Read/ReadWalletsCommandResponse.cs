namespace SyncronizationBot.Application.Response.MainCommands.Read
{
    public class ReadWalletsCommandResponse
    {
        public bool HasWalletsWithBalanceLoad { get; set; }
        public int? TotalValidTransactions { get; set; }
    }
}
