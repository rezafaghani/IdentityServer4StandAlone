using System;
using System.Threading.Tasks;
using Apotheek.Base.AuditLogging.EntityFramework.Entities;
using Apotheek.Base.AuditLogging.EntityFramework.Mapping;
using Apotheek.Base.AuditLogging.EntityFramework.Repositories;
using Apotheek.Base.AuditLogging.Events;
using Apotheek.Base.AuditLogging.Services;

namespace Apotheek.Base.AuditLogging.EntityFramework.Services
{
    public class DatabaseAuditEventLoggerSink<TAuditLog> : IAuditEventLoggerSink 
        where TAuditLog : AuditLog, new()
    {
        private readonly IAuditLoggingRepository<TAuditLog> _auditLoggingRepository;

        public DatabaseAuditEventLoggerSink(IAuditLoggingRepository<TAuditLog> auditLoggingRepository)
        {
            _auditLoggingRepository = auditLoggingRepository;
        }

        public virtual async Task PersistAsync(AuditEvent auditEvent)
        {
            if (auditEvent == null) throw new ArgumentNullException(nameof(auditEvent));

            var auditLog = auditEvent.MapToEntity<TAuditLog>();

            await _auditLoggingRepository.SaveAsync(auditLog);
        }
    }
}