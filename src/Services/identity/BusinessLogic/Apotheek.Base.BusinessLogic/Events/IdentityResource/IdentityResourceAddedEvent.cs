using Apotheek.Base.AuditLogging.Events;
using Apotheek.Base.BusinessLogic.Dtos.Configuration;

namespace Apotheek.Base.BusinessLogic.Events.IdentityResource
{
    public class IdentityResourceAddedEvent : AuditEvent
    {
        public IdentityResourceDto IdentityResource { get; set; }

        public IdentityResourceAddedEvent(IdentityResourceDto identityResource)
        {
            IdentityResource = identityResource;
        }
    }
}