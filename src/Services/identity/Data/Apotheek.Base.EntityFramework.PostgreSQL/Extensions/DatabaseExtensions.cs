using System.Reflection;
using Apotheek.Base.AuditLogging.EntityFramework.DbContexts;
using Apotheek.Base.AuditLogging.EntityFramework.Entities;
using Apotheek.Base.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Apotheek.Base.EntityFramework.PostgreSQL.Extensions
{
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Register DbContexts for IdentityServer ConfigurationStore and PersistedGrants, Identity and Logging
        /// Configure the connection strings in AppSettings.json
        /// </summary>
        /// <typeparam name="TConfigurationDbContext"></typeparam>
        /// <typeparam name="TPersistedGrantDbContext"></typeparam>
        /// <typeparam name="TIdentityDbContext"></typeparam>
        /// <typeparam name="TAuditLoggingDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="identityConnectionString"></param>
        /// <param name="configurationConnectionString"></param>
        /// <param name="persistedGrantConnectionString"></param>
        /// <param name="auditLoggingConnectionString"></param>
        public static void RegisterNpgSqlDbContexts<TIdentityDbContext, TConfigurationDbContext,
            TPersistedGrantDbContext, TAuditLoggingDbContext>(this IServiceCollection services,
            string identityConnectionString,
            string configurationConnectionString,
            string persistedGrantConnectionString,
            string auditLoggingConnectionString)
            where TIdentityDbContext : DbContext
            where TPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
            where TConfigurationDbContext : DbContext, IAdminConfigurationDbContext
            where TAuditLoggingDbContext : DbContext, IAuditLoggingDbContext<AuditLog>
        {
            var migrationsAssembly = typeof(DatabaseExtensions).GetTypeInfo().Assembly.GetName().Name;

            // Config DB for identity
            services.AddDbContext<TIdentityDbContext>(options =>
                options.UseNpgsql(identityConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            // Config DB from existing connection
            services.AddConfigurationDbContext<TConfigurationDbContext>(options =>
                options.ConfigureDbContext = b =>
                    b.UseNpgsql(configurationConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            // Operational DB from existing connection
            services.AddOperationalDbContext<TPersistedGrantDbContext>(options => options.ConfigureDbContext = b =>
                b.UseNpgsql(persistedGrantConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            
            // Audit logging connection
            services.AddDbContext<TAuditLoggingDbContext>(options => options.UseNpgsql(auditLoggingConnectionString,
                optionsSql => optionsSql.MigrationsAssembly(migrationsAssembly)));
        }

        /// <summary>
        /// Register DbContexts for IdentityServer ConfigurationStore and PersistedGrants and Identity
        /// Configure the connection strings in AppSettings.json
        /// </summary>
        /// <typeparam name="TConfigurationDbContext"></typeparam>
        /// <typeparam name="TPersistedGrantDbContext"></typeparam>
        /// <typeparam name="TIdentityDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="identityConnectionString"></param>
        /// <param name="configurationConnectionString"></param>
        /// <param name="persistedGrantConnectionString"></param>
        public static void RegisterNpgSqlDbContexts<TIdentityDbContext, TConfigurationDbContext,
            TPersistedGrantDbContext>(this IServiceCollection services,
            string identityConnectionString, string configurationConnectionString,
            string persistedGrantConnectionString)
            where TIdentityDbContext : DbContext
            where TPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
            where TConfigurationDbContext : DbContext, IAdminConfigurationDbContext
        {
            var migrationsAssembly = typeof(DatabaseExtensions).GetTypeInfo().Assembly.GetName().Name;

            // Config DB for identity
            services.AddDbContext<TIdentityDbContext>(options => options.UseNpgsql(identityConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            // Config DB from existing connection
            services.AddConfigurationDbContext<TConfigurationDbContext>(options => options.ConfigureDbContext = b => b.UseNpgsql(configurationConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            // Operational DB from existing connection
            services.AddOperationalDbContext<TPersistedGrantDbContext>(options => options.ConfigureDbContext = b => b.UseNpgsql(persistedGrantConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));
        }
    }
}