using Microsoft.Extensions.DependencyInjection;

namespace Apotheek.Base.AuditLogging.Extensions
{
    public interface IAuditLoggingBuilder
    {
        IServiceCollection Services { get; }
    }
}