using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Apotheek.Base.ApiGateway.Web.DelegatingHandlers
{
    public class GlobalDelegatingHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization != null && request.Headers.Contains("cpi"))
            {
                var handler = new JwtSecurityTokenHandler();
                var bearerToken = request.Headers.Authorization.Parameter;

                if (!string.IsNullOrEmpty(bearerToken))
                {
                    var token = handler.ReadToken(bearerToken) as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;
                    if (token == null)
                    {
                        return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized));
                    }

                    //var pharmaciesCluster = token.Claims.FirstOrDefault(q => q.Type.ToLower() == "p_cluster");
                    //if(pharmaciesCluster == null)
                    //{
                    //    return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized));
                    //}


                }
            }


            return base.SendAsync(request, cancellationToken);
        }
    }
}
