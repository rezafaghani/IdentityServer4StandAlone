using Apotheek.Base.AuditLogging.Events;
using Apotheek.Base.BusinessLogic.Dtos.Configuration;

namespace Apotheek.Base.BusinessLogic.Events.ApiResource
{
    public class ApiScopeDeletedEvent : AuditEvent
    {
        public ApiScopesDto ApiScope { get; set; }

        public ApiScopeDeletedEvent(ApiScopesDto apiScope)
        {
            ApiScope = apiScope;
        }
    }
}