using Apotheek.Base.AuditLogging.Events;
using Apotheek.Base.BusinessLogic.Dtos.Configuration;


namespace Apotheek.Base.BusinessLogic.Events.Client
{
    public class ClientPropertyAddedEvent : AuditEvent
    {
        public ClientPropertiesDto ClientProperties { get; set; }

        public ClientPropertyAddedEvent(ClientPropertiesDto clientProperties)
        {
            ClientProperties = clientProperties;
        }
    }
}