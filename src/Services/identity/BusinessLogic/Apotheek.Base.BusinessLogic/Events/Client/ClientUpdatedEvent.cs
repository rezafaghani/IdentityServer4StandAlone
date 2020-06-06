using Apotheek.Base.AuditLogging.Events;
using Apotheek.Base.BusinessLogic.Dtos.Configuration;


namespace Apotheek.Base.BusinessLogic.Events.Client
{
    public class ClientUpdatedEvent: AuditEvent
    {
        public ClientDto OriginalClient { get; set; }
        public ClientDto Client { get; set; }

        public ClientUpdatedEvent(ClientDto originalClient, ClientDto client)
        {
            OriginalClient = originalClient;
            Client = client;
        }
    }
}