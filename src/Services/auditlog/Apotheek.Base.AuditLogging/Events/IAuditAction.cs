﻿namespace Apotheek.Base.AuditLogging.Events
{
    public interface IAuditAction
    {
        object Action { get; set; }
    }
}