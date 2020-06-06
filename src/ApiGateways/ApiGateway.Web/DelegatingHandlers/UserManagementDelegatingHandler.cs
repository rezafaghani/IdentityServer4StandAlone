using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Apotheek.Base.ApiGateway.Web.DelegatingHandlers
{
    public class UserManagementDelegatingHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken);
        }
    }
}
