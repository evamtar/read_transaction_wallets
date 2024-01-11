using ReadTransactionsWallets.Domain.Model.Database.Base;

namespace ReadTransactionsWallets.Domain.Model.Database
{
    public class TelegramChannel : Entity
    {
        public decimal? ChannelId { get; set; }
        public string? ChannelName { get; set; }
    }
}
