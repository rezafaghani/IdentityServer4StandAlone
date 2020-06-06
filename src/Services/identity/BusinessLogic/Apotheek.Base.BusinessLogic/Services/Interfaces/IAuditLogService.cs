using System;
using System.Threading.Tasks;
using Apotheek.Base.BusinessLogic.Dtos.Log;

namespace Apotheek.Base.BusinessLogic.Services.Interfaces
{
    public interface IAuditLogService
    {
        Task<AuditLogsDto> GetAsync(AuditLogFilterDto filters);

        Task DeleteLogsOlderThanAsync(DateTime deleteOlderThan);
    }
}
