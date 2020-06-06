using System.Threading.Tasks;
using Apotheek.Base.AuditLogging.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace Apotheek.Base.AuditLogging.EntityFramework.DbContexts
{
    public abstract class AuditLoggingDbContext<TAuditLog> : DbContext, IAuditLoggingDbContext<TAuditLog> 
        where TAuditLog : AuditLog

    {
        protected AuditLoggingDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<TAuditLog> AuditLog { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
