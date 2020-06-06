using Apotheek.Base.AuditLogging.Events;
using Apotheek.Base.BusinessLogic.Dtos.Configuration;

namespace Apotheek.Base.BusinessLogic.Events.ApiResource
{
    public class ApiScopeUpdatedEvent : AuditEvent
    {
        public ApiScopesDto OriginalApiScope { get; set; }
        public ApiScopesDto ApiScope { get; set; }

        public ApiScopeUpdatedEvent(ApiScopesDto originalApiScope, ApiScopesDto apiScope)
        {
            OriginalApiScope = originalApiScope;
            ApiScope = apiScope;
        }
    }
}