using Apotheek.Base.AuditLogging.Events;
using Apotheek.Base.BusinessLogic.Identity.Dtos.Grant;

namespace Apotheek.Base.BusinessLogic.Identity.Events.PersistedGrant
{
    public class PersistedGrantsIdentityByUserRequestedEvent : AuditEvent
    {
        public PersistedGrantsDto PersistedGrants { get; set; }

        public PersistedGrantsIdentityByUserRequestedEvent(PersistedGrantsDto persistedGrants)
        {
            PersistedGrants = persistedGrants;
        }
    }
}