using Apotheek.Base.AuditLogging.Events;
using Apotheek.Base.BusinessLogic.Dtos.Configuration;

namespace Apotheek.Base.BusinessLogic.Events.Client
{
    public class ClientClaimAddedEvent : AuditEvent
    {
        public ClientClaimsDto ClientClaim { get; set; }

        public ClientClaimAddedEvent(ClientClaimsDto clientClaim)
        {
            ClientClaim = clientClaim;
        }
    }
}