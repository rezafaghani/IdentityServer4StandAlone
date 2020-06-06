using System;
using System.Collections.Generic;
using Apotheek.Base.AuditLogging.EntityFramework.Entities;
using Apotheek.Base.BusinessLogic.Extensions;
using Apotheek.Base.BusinessLogic.Identity.Dtos.Identity;
using Apotheek.Base.EntityFramework.Shared.DbContexts;
using Apotheek.Base.EntityFramework.Shared.Entities.Identity;
using Apotheek.Base.Identity.Api.Configuration;
using Apotheek.Base.Identity.Api.Configuration.Authorization;
using Apotheek.Base.Identity.Api.Configuration.Constants;
using Apotheek.Base.Identity.Api.Configuration.Interfaces;
using Apotheek.Base.Identity.Api.Dtos.Users;
using Apotheek.Base.Identity.Api.ExceptionHandling;
using Apotheek.Base.Identity.Api.Helpers;
using Apotheek.Base.Identity.Api.Mappers;
using Apotheek.Base.Identity.Api.Resources;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Apotheek.Base.Identity.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IWebHostEnvironment HostingEnvironment { get; }
        public Startup(IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            
            HostingEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            var adminApiConfiguration = Configuration.GetSection(nameof(AdminConfiguration)).Get<AdminConfiguration>();
            services.AddSingleton(adminApiConfiguration);
            var rootConfiguration = CreateRootConfiguration();
            services.AddSingleton(rootConfiguration);
            // Register DbContexts for IdentityServer and Identity
            RegisterDbContexts(services);

            services.AddScoped<ControllerExceptionFilterAttribute>();
            services.AddScoped<IApiErrorResources, ApiErrorResources>();
            services.AddScoped<IIdentityServerHelper, IdentityServerHelper>();
            
            // Add email senders which is currently setup for SendGrid and SMTP
            services.AddEmailSenders(Configuration);

            // Add services for authentication, including Identity model and external providers
            RegisterAuthentication(services);
            var profileTypes = new HashSet<Type>
            {
                typeof(IdentityMapperProfile<RoleDto<long>, long, UserRolesDto<RoleDto<long>, long, long>, long, UserClaimsDto<long>, UserClaimDto<long>, UserProviderDto<long>, UserProvidersDto<long>, UserChangePasswordDto<long>,RoleClaimDto<long>, RoleClaimsDto<long>>)
            };

            services.AddAdminAspNetIdentityServices<AdminIdentityDbContext, IdentityServerPersistedGrantDbContext, UserDto<long>, long, RoleDto<long>, long, long, long,
                UserIdentity, UserIdentityRole, long, UserIdentityUserClaim, UserIdentityUserRole,
                UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken,
                UsersDto<UserDto<long>, long>, RolesDto<RoleDto<long>, long>, UserRolesDto<RoleDto<long>, long, long>,
                UserClaimsDto<long>, UserProviderDto<long>, UserProvidersDto<long>, UserChangePasswordDto<long>,
                RoleClaimsDto<long>, UserClaimDto<long>, RoleClaimDto<long>>(profileTypes);


            
            services.AddAdminServices<IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext>();

            services.AddAdminApiCors(adminApiConfiguration);

            services.AddMvcServices<UserDto<long>, long, RoleDto<long>, CreateUserDto<long, long>, long, long, long,
                UserIdentity, UserIdentityRole, long, UserIdentityUserClaim, UserIdentityUserRole,
                UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken,
                UsersDto<UserDto<long>, long>, RolesDto<RoleDto<long>, long>, UserRolesDto<RoleDto<long>, long, long>,
                UserClaimsDto<long>, UserProviderDto<long>, UserProvidersDto<long>, UserChangePasswordDto<long>,
                RoleClaimsDto<long>>();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(adminApiConfiguration.ApiVersion, new OpenApiInfo { Title = adminApiConfiguration.ApiName, Version = adminApiConfiguration.ApiVersion });

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            Scopes = new Dictionary<string, string> {
                                { adminApiConfiguration.OidcApiName, adminApiConfiguration.ApiName }
                            },
                            TokenUrl = new Uri($"{adminApiConfiguration.IdentityServerBaseUrl}/connect/token"),

                        }
                    }
                });
                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });

            services.AddAuditEventLogging<AdminAuditLogDbContext, AuditLog>(Configuration);
            // Add authorization policies for MVC
            RegisterAuthorization(services);

            services.AddIdSHealthChecks<IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, AdminIdentityDbContext, AdminAuditLogDbContext>(Configuration, adminApiConfiguration);
            
        }

        public virtual void RegisterAuthentication(IServiceCollection services)
        {
            //var adminApiConfiguration = Configuration.GetSection(nameof(AdminConfiguration)).Get<AdminConfiguration>();
            services.AddAuthenticationServices<AdminIdentityDbContext, UserIdentity, UserIdentityRole>(Configuration);
            services.AddIdentityServer<IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, UserIdentity>(Configuration);
            
            //services.AddAuthentication<AdminIdentityDbContext, UserIdentity, UserIdentityRole>(adminApiConfiguration);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AdminConfiguration adminConfiguration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{adminConfiguration.ApiBaseUrl}/swagger/v1/swagger.json", adminConfiguration.ApiName);

                c.OAuthClientId(adminConfiguration.OidcSwaggerUiClientId);
                c.OAuthAppName(adminConfiguration.ApiName);
            });

            app.UseRouting();
            UseAuthentication(app);
            app.UseCors();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
        public virtual void RegisterDbContexts(IServiceCollection services)
        {
            services.RegisterDbContexts<AdminIdentityDbContext, IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, AdminAuditLogDbContext>(Configuration);
        }
       

        public virtual void RegisterAuthorization(IServiceCollection services)
        {
            var rootConfiguration = CreateRootConfiguration();
            services.AddAuthorizationPolicies(rootConfiguration);
        }

        public virtual void UseAuthentication(IApplicationBuilder app)
        {
            app.UseIdentityServer();
        }

        protected IRootConfiguration CreateRootConfiguration()
        {
            var rootConfiguration = new RootConfiguration();
            Configuration.GetSection(ConfigurationConsts.AdminConfigurationKey).Bind(rootConfiguration.AdminConfiguration);
            Configuration.GetSection(ConfigurationConsts.RegisterConfigurationKey).Bind(rootConfiguration.RegisterConfiguration);
            Configuration.GetSection(ConfigurationConsts.DataConfigurationKey).Bind(rootConfiguration.DataConfiguration);
            Configuration.GetSection(ConfigurationConsts.ServerDataConfigurationKey).Bind(rootConfiguration.ServerDataConfiguration);
            return rootConfiguration;
        }
    }
}
