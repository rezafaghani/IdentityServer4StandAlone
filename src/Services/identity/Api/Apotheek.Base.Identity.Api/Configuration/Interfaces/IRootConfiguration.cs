namespace Apotheek.Base.Identity.Api.Configuration.Interfaces
{
    public interface IRootConfiguration
    {
        AdminConfiguration AdminConfiguration { get; }

        RegisterConfiguration RegisterConfiguration { get; }
        DataConfiguration DataConfiguration { get; }
        ServerDataConfiguration ServerDataConfiguration { get; }
    }
}