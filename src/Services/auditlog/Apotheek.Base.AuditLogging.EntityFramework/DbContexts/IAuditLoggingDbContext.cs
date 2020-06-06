using System.Threading.Tasks;
using Apotheek.Base.AuditLogging.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace Apotheek.Base.AuditLogging.EntityFramework.DbContexts
{
    public interface IAuditLoggingDbContext<TAuditLog> where TAuditLog : AuditLog
    {
        DbSet<TAuditLog> AuditLog { get; set; }

        Task<int> SaveChangesAsync();
    }
}