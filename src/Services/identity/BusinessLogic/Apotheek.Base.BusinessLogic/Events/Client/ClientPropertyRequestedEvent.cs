using Apotheek.Base.AuditLogging.Events;
using Apotheek.Base.BusinessLogic.Dtos.Configuration;


namespace Apotheek.Base.BusinessLogic.Events.Client
{
    public class ClientPropertyRequestedEvent : AuditEvent
    {
        public ClientPropertiesDto ClientProperties { get; set; }

        public ClientPropertyRequestedEvent(ClientPropertiesDto clientProperties)
        {
            ClientProperties = clientProperties;
        }
    }
}