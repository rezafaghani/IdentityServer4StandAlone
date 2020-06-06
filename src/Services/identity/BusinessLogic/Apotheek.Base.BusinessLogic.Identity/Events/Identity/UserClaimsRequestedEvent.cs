using Apotheek.Base.AuditLogging.Events;

namespace Apotheek.Base.BusinessLogic.Identity.Events.Identity
{
    public class UserClaimsRequestedEvent<TUserClaimsDto> : AuditEvent
    {
        public TUserClaimsDto Claims { get; set; }

        public UserClaimsRequestedEvent(TUserClaimsDto claims)
        {
            Claims = claims;
        }
    }
}