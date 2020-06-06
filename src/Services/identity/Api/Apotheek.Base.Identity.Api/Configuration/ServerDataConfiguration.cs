using System.Collections.Generic;
using IdentityServer4.Models;
using Client = Apotheek.Base.Identity.Api.Configuration.IdentityServer.Client;


namespace Apotheek.Base.Identity.Api.Configuration
{
    public class ServerDataConfiguration
    {
        public List<Client> Clients { get; set; } = new List<Client>();
        public List<IdentityResource> Resources { get; set; } = new List<IdentityResource>();
        public List<ApiResource> ApiResources { get; set; } = new List<ApiResource>();
    }
}
