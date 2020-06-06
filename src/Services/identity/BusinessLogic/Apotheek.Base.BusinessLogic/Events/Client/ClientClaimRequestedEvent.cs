using Apotheek.Base.AuditLogging.Events;
using Apotheek.Base.BusinessLogic.Dtos.Configuration;


namespace Apotheek.Base.BusinessLogic.Events.Client
{
    public class ClientClaimRequestedEvent : AuditEvent
    {
        public ClientClaimsDto ClientClaims { get; set; }

        public ClientClaimRequestedEvent(ClientClaimsDto clientClaims)
        {
            ClientClaims = clientClaims;
        }
    }
}