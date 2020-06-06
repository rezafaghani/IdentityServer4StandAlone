using Microsoft.Extensions.DependencyInjection;

namespace Apotheek.Base.AuditLogging.Extensions
{
    public class AuditLoggingBuilder : IAuditLoggingBuilder
    {
        public AuditLoggingBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}