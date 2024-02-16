using SyncronizationBot.Domain.Model.Database.Base;


namespace SyncronizationBot.Domain.Model.Database
{
    public class TypeOperation : Entity
    {
        public string? Name { get; set; }
        public int? IdTypeOperation { get; set; }
        public virtual List<Transactions>? Transactions { get; set; }
        public virtual List<AlertConfiguration>? AlertConfigurations { get; set; }
    }
}
