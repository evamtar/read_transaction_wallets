using SyncronizationBot.Domain.Model.Database.Base;


namespace SyncronizationBot.Domain.Model.Database
{
    public class PublishMessage : Entity
    {
        public Guid? EntityId { get; set; }
        public string? Entity { get; set; }
        public string? JsonValue { get; set; }
        public bool? ItWasPublished { get; set; }
        public Guid? EntityParentId { get; set; }
        public PublishMessage? EntityParent { get; set; }
        public virtual List<PublishMessage>? EntityChildrens { get; set; }
    }
}
