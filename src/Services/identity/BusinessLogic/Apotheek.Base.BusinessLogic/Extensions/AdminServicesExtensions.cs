using Apotheek.Base.BusinessLogic.Resources;
using Apotheek.Base.BusinessLogic.Services;
using Apotheek.Base.BusinessLogic.Services.Interfaces;
using Apotheek.Base.EntityFramework.Interfaces;
using Apotheek.Base.EntityFramework.Repositories;
using Apotheek.Base.EntityFramework.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Apotheek.Base.BusinessLogic.Extensions
{
    public static class AdminServicesExtensions
    {
        public static IServiceCollection AddAdminServices<TAdminDbContext>(
            this IServiceCollection services)
            where TAdminDbContext : DbContext, IAdminPersistedGrantDbContext, IAdminConfigurationDbContext
        {

            return services.AddAdminServices<TAdminDbContext, TAdminDbContext>();
        }

        public static IServiceCollection AddAdminServices<TConfigurationDbContext, TPersistedGrantDbContext>(this IServiceCollection services)
            where TPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
            where TConfigurationDbContext : DbContext, IAdminConfigurationDbContext
            
        {
            //Repositories
            services.AddTransient<IClientRepository, ClientRepository<TConfigurationDbContext>>();
            services.AddTransient<IIdentityResourceRepository, IdentityResourceRepository<TConfigurationDbContext>>();
            services.AddTransient<IApiResourceRepository, ApiResourceRepository<TConfigurationDbContext>>();
            services.AddTransient<IPersistedGrantRepository, PersistedGrantRepository<TPersistedGrantDbContext>>();

            //Services
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IApiResourceService, ApiResourceService>();
            services.AddTransient<IIdentityResourceService, IdentityResourceService>();
            services.AddTransient<IPersistedGrantService, PersistedGrantService>();

            //Resources
            services.AddScoped<IApiResourceServiceResources, ApiResourceServiceResources>();
            services.AddScoped<IClientServiceResources, ClientServiceResources>();
            services.AddScoped<IIdentityResourceServiceResources, IdentityResourceServiceResources>();
            services.AddScoped<IPersistedGrantServiceResources, PersistedGrantServiceResources>();

            return services;
        }
    }
}
