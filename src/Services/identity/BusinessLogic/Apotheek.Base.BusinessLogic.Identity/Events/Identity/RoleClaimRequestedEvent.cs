using Apotheek.Base.AuditLogging.Events;

namespace Apotheek.Base.BusinessLogic.Identity.Events.Identity
{
    public class RoleClaimRequestedEvent<TRoleClaimsDto> : AuditEvent
    {
        public TRoleClaimsDto RoleClaim { get; set; }

        public RoleClaimRequestedEvent(TRoleClaimsDto roleClaim)
        {
            RoleClaim = roleClaim;
        }
    }
}