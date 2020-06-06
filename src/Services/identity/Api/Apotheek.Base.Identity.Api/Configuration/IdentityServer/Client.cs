using System.Collections.Generic;
using System.Security.Claims;

namespace Apotheek.Base.Identity.Api.Configuration.IdentityServer
{
    public class Client : global::IdentityServer4.Models.Client
    {
        public List<Claim> ClientClaims { get; set; } = new List<Claim>();
    }
}
