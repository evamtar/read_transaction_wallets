

namespace SyncronizationBot.Domain.Model.Alerts
{
    public class LogExecute
    {
        public string? ServiceName { get; set; }
        public DateTime? DateExecuted { get; set; }
        public TimeSpan? Timer { get; set; }
    }
}
