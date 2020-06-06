using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;


namespace Apotheek.Base.Identity.Api.Services
{
    public class AuditEventSink : DefaultEventSink
    {
        public AuditEventSink(ILogger<DefaultEventService> logger) : base(logger)
        {
        }
    }
}