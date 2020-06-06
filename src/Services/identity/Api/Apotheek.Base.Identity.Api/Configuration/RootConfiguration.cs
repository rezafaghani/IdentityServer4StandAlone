using Apotheek.Base.Identity.Api.Configuration.Interfaces;

namespace Apotheek.Base.Identity.Api.Configuration
{
    public class RootConfiguration : IRootConfiguration
    {
        public AdminConfiguration AdminConfiguration { get; } = new AdminConfiguration();
        public RegisterConfiguration RegisterConfiguration { get; } = new RegisterConfiguration();
        public DataConfiguration DataConfiguration { get; } = new DataConfiguration();
        public ServerDataConfiguration ServerDataConfiguration { get; } = new ServerDataConfiguration();
    }
}