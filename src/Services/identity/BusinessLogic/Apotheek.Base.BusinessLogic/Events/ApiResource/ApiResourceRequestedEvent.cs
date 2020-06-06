using Apotheek.Base.AuditLogging.Events;
using Apotheek.Base.BusinessLogic.Dtos.Configuration;

namespace Apotheek.Base.BusinessLogic.Events.ApiResource
{
    public class ApiResourceRequestedEvent : AuditEvent
    {
        public int ApiResourceId { get; set; }
        public ApiResourceDto ApiResource { get; set; }

        public ApiResourceRequestedEvent(int apiResourceId, ApiResourceDto apiResource)
        {
            ApiResourceId = apiResourceId;
            ApiResource = apiResource;
        }
    }
}