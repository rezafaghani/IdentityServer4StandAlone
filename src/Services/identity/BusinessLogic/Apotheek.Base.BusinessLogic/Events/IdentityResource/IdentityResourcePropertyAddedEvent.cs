using Apotheek.Base.AuditLogging.Events;
using Apotheek.Base.BusinessLogic.Dtos.Configuration;

namespace Apotheek.Base.BusinessLogic.Events.IdentityResource
{
    public class IdentityResourcePropertyAddedEvent : AuditEvent
    {
        public IdentityResourcePropertiesDto IdentityResourceProperty { get; set; }

        public IdentityResourcePropertyAddedEvent(IdentityResourcePropertiesDto identityResourceProperty)
        {
            IdentityResourceProperty = identityResourceProperty;
        }
    }
}