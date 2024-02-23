

namespace SyncronizationBots.RabbitMQ.Exceptions
{
    public class RelationShipInsertException : Exception
    {
        public RelationShipInsertException(string message, Exception ex) : base(message, ex)
        {
            
        }
    }
}
