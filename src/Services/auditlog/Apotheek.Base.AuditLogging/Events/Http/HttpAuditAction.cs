using Apotheek.Base.AuditLogging.Configuration;
using Apotheek.Base.AuditLogging.Helpers.HttpContextHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace Apotheek.Base.AuditLogging.Events.Http
{
    public class HttpAuditAction : IAuditAction
    {
        public HttpAuditAction(IHttpContextAccessor accessor, AuditHttpActionOptions options)
        {
            Action = new
            {
                accessor.HttpContext.TraceIdentifier,
                RequestUrl = accessor.HttpContext.Request.GetDisplayUrl(),
                HttpMethod = accessor.HttpContext.Request.Method,
                FormVariables = options.IncludeFormVariables ? HttpContextHelpers.GetFormVariables(accessor.HttpContext) : null
            };
        }

        public object Action { get; set; }
    }
}