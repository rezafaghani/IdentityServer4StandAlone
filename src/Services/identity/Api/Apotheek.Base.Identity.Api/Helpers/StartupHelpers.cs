using System;
using System.Globalization;
using System.Linq;
using Apotheek.Base.AuditLogging.EntityFramework.DbContexts;
using Apotheek.Base.AuditLogging.EntityFramework.Entities;
using Apotheek.Base.AuditLogging.EntityFramework.Repositories;
using Apotheek.Base.AuditLogging.EntityFramework.Services;
using Apotheek.Base.AuditLogging.Extensions;
using Apotheek.Base.BusinessLogic.Identity.Dtos.Identity;
using Apotheek.Base.EntityFramework.Helpers;
using Apotheek.Base.EntityFramework.Interfaces;
using Apotheek.Base.EntityFramework.PostgreSQL.Extensions;
using Apotheek.Base.EntityFramework.Shared.Configuration;
using Apotheek.Base.EntityFramework.Shared.DbContexts;
using Apotheek.Base.EntityFramework.Shared.Entities.Identity;
using Apotheek.Base.Identity.Api.Configuration;
using Apotheek.Base.Identity.Api.Configuration.ApplicationParts;
using Apotheek.Base.Identity.Api.Configuration.AuditLogging;
using Apotheek.Base.Identity.Api.Configuration.Constants;
using Apotheek.Base.Identity.Api.Configuration.Interfaces;
using Apotheek.Base.Identity.Api.Dtos.Users;
using Apotheek.Base.Identity.Api.Helpers.Localization;
using Apotheek.Base.Identity.Api.Services;
using IdentityModel;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using SendGrid;

namespace Apotheek.Base.Identity.Api.Helpers
{
    public static class StartupHelpers
    {
        /// <summary>
        /// Register services for MVC and localization including available languages
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static IMvcBuilder AddMvcWithLocalization<TUserDto, TUserDtoKey, TRoleDto, TRoleDtoKey, TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
            TUsersDto, TRolesDto, TCreateUserDto, TUserRolesDto, TUserClaimsDto,
            TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto>(this IServiceCollection services, IConfiguration configuration)
            where TUserDto : UserDto<TUserDtoKey>, new()
            where TRoleDto : RoleDto<TRoleDtoKey>, new()
            where TCreateUserDto : CreateUserDto<TUserDtoKey, TRoleDtoKey>, new()
            where TUser : UserIdentity
            where TRole : UserIdentityRole
            where TKey : IEquatable<TKey>
            where TUserClaim : UserIdentityUserClaim
            where TUserRole : UserIdentityUserRole
            where TUserLogin : IdentityUserLogin<long>
            where TRoleClaim : UserIdentityRoleClaim
            where TUserToken : UserIdentityUserToken
            where TRoleDtoKey : IEquatable<TRoleDtoKey>
            where TUserDtoKey : IEquatable<TUserDtoKey>
            where TUsersDto : UsersDto<TUserDto, TUserDtoKey>
            where TRolesDto : RolesDto<TRoleDto, TRoleDtoKey>
            where TUserRolesDto : UserRolesDto<TRoleDto, TUserDtoKey, TRoleDtoKey>
            where TUserClaimsDto : UserClaimsDto<TUserDtoKey>
            where TUserProviderDto : UserProviderDto<TUserDtoKey>
            where TUserProvidersDto : UserProvidersDto<TUserDtoKey>
            where TUserChangePasswordDto : UserChangePasswordDto<TUserDtoKey>
            where TRoleClaimsDto : RoleClaimsDto<TRoleDtoKey>
        {
            services.AddLocalization(opts => { opts.ResourcesPath = ConfigurationConsts.ResourcesPath; });

            services.TryAddTransient(typeof(IGenericControllerLocalizer<>), typeof(GenericControllerLocalizer<>));

            var mvcBuilder = services.AddControllersWithViews(o =>
                {
                    o.Conventions.Add(new GenericControllerRouteConvention());
                })
                .AddViewLocalization(
                    LanguageViewLocationExpanderFormat.Suffix,
                    opts => { opts.ResourcesPath = ConfigurationConsts.ResourcesPath; })
                .AddDataAnnotationsLocalization()
                .ConfigureApplicationPartManager(m =>
                {
                    m.FeatureProviders.Add(new GenericTypeControllerFeatureProvider<TUserDto, TUserDtoKey, TRoleDto, TCreateUserDto, TRoleDtoKey, TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
                        TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
                        TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto>());
                });

            var cultureConfiguration = configuration.GetSection(nameof(CultureConfiguration)).Get<CultureConfiguration>();
            services.Configure<RequestLocalizationOptions>(
                opts =>
                {
                    // If cultures are specified in the configuration, use them (making sure they are among the available cultures),
                    // otherwise use all the available cultures
                    var supportedCultureCodes = (cultureConfiguration?.Cultures?.Count > 0 ?
                        cultureConfiguration.Cultures.Intersect(CultureConfiguration.AvailableCultures) :
                        CultureConfiguration.AvailableCultures).ToArray();

                    if (!supportedCultureCodes.Any()) supportedCultureCodes = CultureConfiguration.AvailableCultures;
                    var supportedCultures = supportedCultureCodes.Select(c => new CultureInfo(c)).ToList();

                    // If the default culture is specified use it, otherwise use CultureConfiguration.DefaultRequestCulture ("en")
                    var defaultCultureCode = string.IsNullOrEmpty(cultureConfiguration?.DefaultCulture) ?
                        CultureConfiguration.DefaultRequestCulture : cultureConfiguration?.DefaultCulture;

                    // If the default culture is not among the supported cultures, use the first supported culture as default
                    if (!supportedCultureCodes.Contains(defaultCultureCode)) defaultCultureCode = supportedCultureCodes.FirstOrDefault();

                    opts.DefaultRequestCulture = new RequestCulture(defaultCultureCode);
                    opts.SupportedCultures = supportedCultures;
                    opts.SupportedUICultures = supportedCultures;
                });

            return mvcBuilder;
        }

        /// <summary>
        /// Using of Forwarded Headers and Referrer Policy
        /// </summary>
        /// <param name="app"></param>
        public static void UseSecurityHeaders(this IApplicationBuilder app)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions()
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseHsts(options => options.MaxAge(days: 365));
            app.UseReferrerPolicy(options => options.NoReferrer());
        }

        /// <summary>
        /// Add email senders - configuration of sendgrid, smtp senders
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddEmailSenders(this IServiceCollection services, IConfiguration configuration)
        {
            var smtpConfiguration = configuration.GetSection(nameof(SmtpConfiguration)).Get<SmtpConfiguration>();
            var sendGridConfiguration = configuration.GetSection(nameof(SendgridConfiguration)).Get<SendgridConfiguration>();

            if (sendGridConfiguration != null && !string.IsNullOrWhiteSpace(sendGridConfiguration.ApiKey))
            {
                services.AddSingleton<ISendGridClient>(_ => new SendGridClient(sendGridConfiguration.ApiKey));
                services.AddSingleton(sendGridConfiguration);
                services.AddTransient<IEmailSender, SendgridEmailSender>();
            }
            else if (smtpConfiguration != null && !string.IsNullOrWhiteSpace(smtpConfiguration.Host))
            {
                services.AddSingleton(smtpConfiguration);
                services.AddTransient<IEmailSender, SmtpEmailSender>();
            }
            else
            {
                services.AddSingleton<IEmailSender, EmailSender>();
            }
        }

        /// <summary>
        /// Register DbContexts for IdentityServer ConfigurationStore and PersistedGrants, Identity and Logging
        /// Configure the connection strings in AppSettings.json
        /// </summary>
        /// <typeparam name="TConfigurationDbContext"></typeparam>
        /// <typeparam name="TPersistedGrantDbContext"></typeparam>
        /// <typeparam name="TIdentityDbContext"></typeparam>
        /// <typeparam name="TAuditLoggingDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void RegisterDbContexts<TIdentityDbContext, TConfigurationDbContext, TPersistedGrantDbContext, TAuditLoggingDbContext>(this IServiceCollection services, IConfiguration configuration)
            where TIdentityDbContext : DbContext
            where TPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
            where TConfigurationDbContext : DbContext, IAdminConfigurationDbContext
            where TAuditLoggingDbContext : DbContext, IAuditLoggingDbContext<AuditLog>
        {
            var databaseProvider = configuration.GetSection(nameof(DatabaseProviderConfiguration)).Get<DatabaseProviderConfiguration>();

            var identityConnectionString = configuration.GetConnectionString(ConfigurationConsts.IdentityDbConnectionStringKey);
            var configurationConnectionString = configuration.GetConnectionString(ConfigurationConsts.ConfigurationDbConnectionStringKey);
            var persistedGrantsConnectionString = configuration.GetConnectionString(ConfigurationConsts.PersistedGrantDbConnectionStringKey);
            var auditLoggingConnectionString = configuration.GetConnectionString(ConfigurationConsts.AdminAuditLogDbConnectionStringKey);

            switch (databaseProvider.ProviderType)
            {

                case DatabaseProviderType.PostgreSql:
                    services.RegisterNpgSqlDbContexts<TIdentityDbContext, TConfigurationDbContext, TPersistedGrantDbContext, TAuditLoggingDbContext>(identityConnectionString, configurationConnectionString, persistedGrantsConnectionString, auditLoggingConnectionString);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(databaseProvider.ProviderType), $@"The value needs to be one of {string.Join(", ", Enum.GetNames(typeof(DatabaseProviderType)))}.");
            }
        }

        /// <summary>
        /// Register in memory DbContexts for IdentityServer ConfigurationStore and PersistedGrants, Identity and Logging
        /// For testing purpose only
        /// </summary>
        /// <typeparam name="TConfigurationDbContext"></typeparam>
        /// <typeparam name="TPersistedGrantDbContext"></typeparam>
        /// <typeparam name="TIdentityDbContext"></typeparam>
        /// <typeparam name="TAuditLoggingDbContext"></typeparam>
        /// <param name="services"></param>
        public static void RegisterDbContextsStaging<TIdentityDbContext, TConfigurationDbContext, TPersistedGrantDbContext, TAuditLoggingDbContext>(this IServiceCollection services)
            where TIdentityDbContext : DbContext
            where TPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
            where TConfigurationDbContext : DbContext, IAdminConfigurationDbContext
            where TAuditLoggingDbContext : DbContext, IAuditLoggingDbContext<AuditLog>
        {
            var persistedGrantsDatabaseName = Guid.NewGuid().ToString();
            var configurationDatabaseName = Guid.NewGuid().ToString();
            var identityDatabaseName = Guid.NewGuid().ToString();
            var auditLoggingDatabaseName = Guid.NewGuid().ToString();

            var operationalStoreOptions = new OperationalStoreOptions();
            services.AddSingleton(operationalStoreOptions);

            var storeOptions = new ConfigurationStoreOptions();
            services.AddSingleton(storeOptions);

            services.AddDbContext<TIdentityDbContext>(optionsBuilder => optionsBuilder.UseInMemoryDatabase(identityDatabaseName));
            services.AddDbContext<TPersistedGrantDbContext>(optionsBuilder => optionsBuilder.UseInMemoryDatabase(persistedGrantsDatabaseName));
            services.AddDbContext<TConfigurationDbContext>(optionsBuilder => optionsBuilder.UseInMemoryDatabase(configurationDatabaseName));
            services.AddDbContext<TAuditLoggingDbContext>(optionsBuilder => optionsBuilder.UseInMemoryDatabase(auditLoggingDatabaseName));
        }

        /// <summary>
        /// Add services for authentication, including Identity model, IdentityServer4 and external providers
        /// </summary>
        /// <typeparam name="TIdentityDbContext">DbContext for Identity</typeparam>
        /// <typeparam name="TUserIdentity">User Identity class</typeparam>
        /// <typeparam name="TUserIdentityRole">User Identity Role class</typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddAuthenticationServices<TIdentityDbContext, TUserIdentity, TUserIdentityRole>(this IServiceCollection services, IConfiguration configuration) where TIdentityDbContext : DbContext
            where TUserIdentity : class
            where TUserIdentityRole : class
        {
            var loginConfiguration = GetLoginConfiguration(configuration);
            var registrationConfiguration = GetRegistrationConfiguration(configuration);

            services
                .AddSingleton(registrationConfiguration)
                .AddSingleton(loginConfiguration)
                .AddScoped<UserResolver<TUserIdentity>>();
                //.AddIdentity<TUserIdentity, TUserIdentityRole>(options =>
                //{
                //    options.User.RequireUniqueEmail = true;
                //})
                //.AddEntityFrameworkStores<TIdentityDbContext>()
                //.AddDefaultTokenProviders();

            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });
            var adminApiConfiguration = configuration.GetSection(nameof(AdminConfiguration)).Get<AdminConfiguration>();
            var authenticationBuilder = AddAuthentication<AdminIdentityDbContext, UserIdentity, UserIdentityRole>(services,adminApiConfiguration);
            
            AddExternalProviders(authenticationBuilder, configuration);
        }
        /// <summary>
        /// Add authentication middleware for an API
        /// </summary>
        /// <typeparam name="TIdentityDbContext">DbContext for an access to Identity</typeparam>
        /// <typeparam name="TUser">Entity with User</typeparam>
        /// <typeparam name="TRole">Entity with Role</typeparam>
        /// <param name="services"></param>
        /// <param name="adminApiConfiguration"></param>
        public static AuthenticationBuilder AddAuthentication<TIdentityDbContext, TUser, TRole>(this IServiceCollection services,
            AdminConfiguration adminApiConfiguration)
            where TIdentityDbContext : DbContext
            where TRole : class
            where TUser : class
        {
            services.AddIdentity<TUser, TRole>(options => { options.User.RequireUniqueEmail = true; })
                .AddEntityFrameworkStores<TIdentityDbContext>()
                .AddDefaultTokenProviders();

          var builder=  services.AddAuthentication(options =>
                {
                    options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultForbidScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                })
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = adminApiConfiguration.IdentityServerBaseUrl;
                    options.ApiName = adminApiConfiguration.OidcApiName;
                    options.RequireHttpsMetadata = adminApiConfiguration.RequireHttpsMetadata;
                    options.SaveToken = true;
                    options.EnableCaching = true;
                    options.CacheDuration = TimeSpan.FromHours(12);

                });
          return builder;
        }

        /// <summary>
        /// Get configuration for login
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static LoginConfiguration GetLoginConfiguration(IConfiguration configuration)
        {
            var loginConfiguration = configuration.GetSection(nameof(LoginConfiguration)).Get<LoginConfiguration>();

            // Cannot load configuration - use default configuration values
            if (loginConfiguration == null)
            {
                return new LoginConfiguration();
            }

            return loginConfiguration;
        }

        /// <summary>
        /// Get configuration for registration
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static RegisterConfiguration GetRegistrationConfiguration(IConfiguration configuration)
        {
            var registerConfiguration = configuration.GetSection(nameof(RegisterConfiguration)).Get<RegisterConfiguration>();

            // Cannot load configuration - use default configuration values
            if (registerConfiguration == null)
            {
                return new RegisterConfiguration();
            }

            return registerConfiguration;
        }

        /// <summary>
        /// Add configuration for IdentityServer4
        /// </summary>
        /// <typeparam name="TUserIdentity"></typeparam>
        /// <typeparam name="TConfigurationDbContext"></typeparam>
        /// <typeparam name="TPersistedGrantDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static IIdentityServerBuilder AddIdentityServer<TConfigurationDbContext, TPersistedGrantDbContext, TUserIdentity>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
            where TConfigurationDbContext : DbContext, IAdminConfigurationDbContext
            where TUserIdentity : class
        {
            var builder = services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddConfigurationStore<TConfigurationDbContext>()
                .AddOperationalStore<TPersistedGrantDbContext>()
                .AddAspNetIdentity<TUserIdentity>();

            builder.AddCustomSigningCredential(configuration);
            builder.AddCustomValidationKey(configuration);

            return builder;
        }

        /// <summary>
        /// Add external providers
        /// </summary>
        /// <param name="authenticationBuilder"></param>
        /// <param name="configuration"></param>
        private static void AddExternalProviders(AuthenticationBuilder authenticationBuilder,
            IConfiguration configuration)
        {
            var externalProviderConfiguration = configuration.GetSection(nameof(ExternalProvidersConfiguration)).Get<ExternalProvidersConfiguration>();

            if (externalProviderConfiguration.UseGitHubProvider)
            {
                authenticationBuilder.AddGitHub(options =>
                {
                    options.ClientId = externalProviderConfiguration.GitHubClientId;
                    options.ClientSecret = externalProviderConfiguration.GitHubClientSecret;
                    options.Scope.Add("user:email");
                });
            }
        }

        /// <summary>
        /// Register middleware for localization
        /// </summary>
        /// <param name="app"></param>
        public static void UseMvcLocalizationServices(this IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);
        }

        /// <summary>
        /// Add authorization policies
        /// </summary>
        /// <param name="services"></param>
        /// <param name="rootConfiguration"></param>
        public static void AddAuthorizationPolicies(this IServiceCollection services,
                IRootConfiguration rootConfiguration)
        {
            var adminApiConfiguration = services.BuildServiceProvider().GetService<AdminConfiguration>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationConsts.AdministrationPolicy,
                    policy =>
                        policy.RequireAssertion(context => context.User.HasClaim(c =>
                                (c.Type == JwtClaimTypes.Role && c.Value == adminApiConfiguration.AdministrationRole) ||
                                (c.Type == $"client_{JwtClaimTypes.Role}" && c.Value == adminApiConfiguration.AdministrationRole)
                            )
                        ));
            });
        }

        public static void AddIdSHealthChecks<TConfigurationDbContext, TPersistedGrantDbContext, TIdentityDbContext, TAuditLoggingDbContext>(this IServiceCollection services, IConfiguration configuration, AdminConfiguration adminConfiguration)
     where TConfigurationDbContext : DbContext, IAdminConfigurationDbContext
     where TPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
     where TIdentityDbContext : DbContext
     where TAuditLoggingDbContext : DbContext, IAuditLoggingDbContext<AuditLog>
        {
            var configurationDbConnectionString = configuration.GetConnectionString(ConfigurationConsts.ConfigurationDbConnectionStringKey);
            var persistedGrantsDbConnectionString = configuration.GetConnectionString(ConfigurationConsts.PersistedGrantDbConnectionStringKey);
            var identityDbConnectionString = configuration.GetConnectionString(ConfigurationConsts.IdentityDbConnectionStringKey);
            var auditLogDbConnectionString = configuration.GetConnectionString(ConfigurationConsts.AdminAuditLogDbConnectionStringKey);

            var identityServerUri = adminConfiguration.IdentityServerBaseUrl;
            var healthChecksBuilder = services.AddHealthChecks()
                .AddDbContextCheck<TConfigurationDbContext>("ConfigurationDbContext")
                .AddDbContextCheck<TPersistedGrantDbContext>("PersistedGrantsDbContext")
                .AddDbContextCheck<TIdentityDbContext>("IdentityDbContext")
                .AddDbContextCheck<TAuditLoggingDbContext>("AuditLogDbContext")
                .AddIdentityServer(new Uri(identityServerUri), "Identity Server");

            var serviceProvider = services.BuildServiceProvider();
            var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var configurationTableName = DbContextHelpers.GetEntityTable<TConfigurationDbContext>(scope.ServiceProvider);
                var persistedGrantTableName = DbContextHelpers.GetEntityTable<TPersistedGrantDbContext>(scope.ServiceProvider);
                var identityTableName = DbContextHelpers.GetEntityTable<TIdentityDbContext>(scope.ServiceProvider);
                var auditLogTableName = DbContextHelpers.GetEntityTable<TAuditLoggingDbContext>(scope.ServiceProvider);

                var databaseProvider = configuration.GetSection(nameof(DatabaseProviderConfiguration)).Get<DatabaseProviderConfiguration>();
                switch (databaseProvider.ProviderType)
                {

                    case DatabaseProviderType.PostgreSql:
                        healthChecksBuilder
                            .AddNpgSql(configurationDbConnectionString, name: "ConfigurationDb",
                                healthQuery: $"SELECT * FROM {configurationTableName} LIMIT 1")
                            .AddNpgSql(persistedGrantsDbConnectionString, name: "PersistentGrantsDb",
                                healthQuery: $"SELECT * FROM {persistedGrantTableName} LIMIT 1")
                            .AddNpgSql(identityDbConnectionString, name: "IdentityDb",
                                healthQuery: $"SELECT * FROM {identityTableName} LIMIT 1")
                            .AddNpgSql(auditLogDbConnectionString, name: "AuditLogDb",
                                healthQuery: $"SELECT * FROM {auditLogTableName}  LIMIT 1");
                        break;

                    default:
                        throw new NotImplementedException($"Health checks not defined for database provider {databaseProvider.ProviderType}");
                }
            }
        }
        public static IServiceCollection AddAuditEventLogging<TAuditLoggingDbContext, TAuditLog>(
            this IServiceCollection services, IConfiguration configuration)
            where TAuditLog : AuditLog, new()
            where TAuditLoggingDbContext : IAuditLoggingDbContext<TAuditLog>
        {
            var auditLoggingConfiguration = configuration.GetSection(nameof(AuditLoggingConfiguration))
                .Get<AuditLoggingConfiguration>();
            services.AddSingleton(auditLoggingConfiguration);

            services.AddAuditLogging(options => { options.Source = auditLoggingConfiguration.Source; })
                .AddEventData<ApiAuditSubject, ApiAuditAction>()
                .AddAuditSinks<DatabaseAuditEventLoggerSink<TAuditLog>>();

            services
                .AddTransient<IAuditLoggingRepository<TAuditLog>,
                    AuditLoggingRepository<TAuditLoggingDbContext, TAuditLog>>();

            return services;
        }

        public static IServiceCollection AddAdminApiCors(this IServiceCollection services, AdminConfiguration adminApiConfiguration)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        if (adminApiConfiguration.CorsAllowAnyOrigin)
                        {
                            builder.AllowAnyOrigin();
                        }
                        else
                        {
                            builder.WithOrigins(adminApiConfiguration.CorsAllowOrigins);
                        }

                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                    });
            });

            return services;
        }

        /// <summary>
        /// Register services for MVC
        /// </summary>
        /// <param name="services"></param>
        public static void AddMvcServices<TUserDto, TUserDtoKey, TRoleDto, TCreateUserDto, TRoleDtoKey, TUserKey, TRoleKey,
            TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
            TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
            TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto>(
            this IServiceCollection services)
            where TUserDto : UserDto<TUserDtoKey>, new()
            where TRoleDto : RoleDto<TRoleDtoKey>, new()
            where TCreateUserDto : CreateUserDto<TUserDtoKey, TRoleDtoKey>, new()
            where TUser : UserIdentity
            where TRole : UserIdentityRole
            where TKey : IEquatable<TKey>
            where TUserClaim : UserIdentityUserClaim
            where TUserRole : UserIdentityUserRole
            where TUserLogin : IdentityUserLogin<long>
            where TRoleClaim : UserIdentityRoleClaim
            where TUserToken : UserIdentityUserToken
            where TRoleDtoKey : IEquatable<TRoleDtoKey>
            where TUserDtoKey : IEquatable<TUserDtoKey>
            where TUsersDto : UsersDto<TUserDto, TUserDtoKey>
            where TRolesDto : RolesDto<TRoleDto, TRoleDtoKey>
            where TUserRolesDto : UserRolesDto<TRoleDto, TUserDtoKey, TRoleDtoKey>
            where TUserClaimsDto : UserClaimsDto<TUserDtoKey>
            where TUserProviderDto : UserProviderDto<TUserDtoKey>
            where TUserProvidersDto : UserProvidersDto<TUserDtoKey>
            where TUserChangePasswordDto : UserChangePasswordDto<TUserDtoKey>
            where TRoleClaimsDto : RoleClaimsDto<TRoleDtoKey>
        {
            services.TryAddTransient(typeof(IGenericControllerLocalizer<>), typeof(GenericControllerLocalizer<>));

            services.AddControllersWithViews(o => { o.Conventions.Add(new GenericControllerRouteConvention()); })
                .AddDataAnnotationsLocalization()
                .ConfigureApplicationPartManager(m =>
                {
                    m.FeatureProviders.Add(
                        new GenericTypeControllerFeatureProvider<TUserDto, TUserDtoKey, TRoleDto, TCreateUserDto, TRoleDtoKey, TUserKey,
                            TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
                            TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
                            TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto>());
                });
        }

       
       

    }
}
