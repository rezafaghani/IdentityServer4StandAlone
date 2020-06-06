using Apotheek.Base.AuditLogging.EntityFramework.Entities;
using Apotheek.Base.AuditLogging.Events;
using Apotheek.Base.AuditLogging.Helpers.JsonHelpers;

namespace Apotheek.Base.AuditLogging.EntityFramework.Mapping
{
    public static class AuditMapping
    {
        public static TAuditLog MapToEntity<TAuditLog>(this AuditEvent auditEvent)
        where TAuditLog : AuditLog, new()
        {
            var auditLog = new TAuditLog
            {
                Event = auditEvent.Event,
                Source = auditEvent.Source,
                SubjectIdentifier = auditEvent.SubjectIdentifier,
                SubjectName = auditEvent.SubjectName,
                SubjectType = auditEvent.SubjectType,
                Category = auditEvent.Category,
                Data = AuditLogSerializer.Serialize(auditEvent, AuditLogSerializer.BaseAuditEventJsonSettings),
                Action = auditEvent.Action == null ? null : AuditLogSerializer.Serialize(auditEvent.Action),
                SubjectAdditionalData = auditEvent.SubjectAdditionalData == null ? null : AuditLogSerializer.Serialize(auditEvent.SubjectAdditionalData)
            };

            return auditLog;
        }
    }
}