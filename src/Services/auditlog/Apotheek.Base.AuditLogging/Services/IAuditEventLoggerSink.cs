
using System.Threading.Tasks;
using Apotheek.Base.AuditLogging.Events;

namespace Apotheek.Base.AuditLogging.Services
{
    public interface IAuditEventLoggerSink
    {
        Task PersistAsync(AuditEvent auditEvent);
    }
}