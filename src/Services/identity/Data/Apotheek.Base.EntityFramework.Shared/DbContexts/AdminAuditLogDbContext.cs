using System.Threading.Tasks;
using Apotheek.Base.AuditLogging.EntityFramework.DbContexts;
using Apotheek.Base.AuditLogging.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;


namespace Apotheek.Base.EntityFramework.Shared.DbContexts
{
    public class AdminAuditLogDbContext : DbContext, IAuditLoggingDbContext<AuditLog>
    {
        public AdminAuditLogDbContext(DbContextOptions<AdminAuditLogDbContext> dbContextOptions)
            : base(dbContextOptions)
        {

        }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        public DbSet<AuditLog> AuditLog { get; set; }
    }
}
