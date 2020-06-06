using Apotheek.Base.AuditLogging.Events;

namespace Apotheek.Base.BusinessLogic.Events.PersistedGrant
{
    public class PersistedGrantsDeletedEvent : AuditEvent
    {
        public string UserId { get; set; }

        public PersistedGrantsDeletedEvent(string userId)
        {
            UserId = userId;
        }
    }
}