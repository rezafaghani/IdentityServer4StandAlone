using Apotheek.Base.AuditLogging.Events;

namespace Apotheek.Base.BusinessLogic.Identity.Events.Identity
{
    public class RoleRequestedEvent<TRoleDto> : AuditEvent
    {
        public TRoleDto Role { get; set; }

        public RoleRequestedEvent(TRoleDto role)
        {
            Role = role;
        }
    }
}