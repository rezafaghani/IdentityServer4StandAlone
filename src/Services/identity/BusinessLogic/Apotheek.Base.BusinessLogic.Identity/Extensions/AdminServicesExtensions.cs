using System;
using System.Collections.Generic;
using Apotheek.Base.BusinessLogic.Identity.Dtos.Identity;
using Apotheek.Base.BusinessLogic.Identity.Mappers.Configuration;
using Apotheek.Base.BusinessLogic.Identity.Resources;
using Apotheek.Base.BusinessLogic.Identity.Services;
using Apotheek.Base.BusinessLogic.Identity.Services.Interfaces;
using Apotheek.Base.EntityFramework.Identity.Repositories;
using Apotheek.Base.EntityFramework.Identity.Repositories.Interfaces;
using Apotheek.Base.EntityFramework.Interfaces;
using Apotheek.Base.EntityFramework.Shared.Entities.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class AdminServicesExtensions
    {
        public static IMapperConfigurationBuilder AddAdminAspNetIdentityMapping(this IServiceCollection services)
        {
            var builder = new MapperConfigurationBuilder();

            services.AddSingleton<IConfigurationProvider>(sp => new MapperConfiguration(cfg =>
            {
                foreach (var profileType in builder.ProfileTypes)
                    cfg.AddProfile(profileType);
            }));

            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService));

            return builder;
        }

        public static IServiceCollection AddAdminAspNetIdentityServices<TIdentityDbContext, TPersistedGrantDbContext, TUser>(
            this IServiceCollection services)
            where TIdentityDbContext : IdentityDbContext<UserIdentity, UserIdentityRole, long,
                UserIdentityUserClaim, UserIdentityUserRole, UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken>
            where TPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
            where TUser : UserIdentity
        {
            return services.AddAdminAspNetIdentityServices<TIdentityDbContext, TPersistedGrantDbContext, UserDto<long>, long, RoleDto<long>, long, long,
                long, TUser, UserIdentityRole, long, UserIdentityUserClaim, UserIdentityUserRole, IdentityUserLogin<long>, UserIdentityRoleClaim, UserIdentityUserToken,
                UsersDto<UserDto<long>, long>, RolesDto<RoleDto<long>, long>, UserRolesDto<RoleDto<long>, long, long>,
                UserClaimsDto<long>, UserProviderDto<long>, UserProvidersDto<long>, UserChangePasswordDto<long>,
                RoleClaimsDto<long>, UserClaimDto<long>, RoleClaimDto<long>>();
        }

        public static IServiceCollection AddAdminAspNetIdentityServices<TAdminDbContext, TUserDto, TUserDtoKey, TRoleDto, TRoleDtoKey, TUserKey,
            TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
            TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
            TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto,
            TUserClaimDto, TRoleClaimDto>(
                this IServiceCollection services)
            where TAdminDbContext :
            IdentityDbContext<UserIdentity, UserIdentityRole, long,
                UserIdentityUserClaim, UserIdentityUserRole, UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken>,
            IAdminPersistedGrantDbContext
            where TUserDto : UserDto<TUserDtoKey>
            where TUser : UserIdentity
            where TRole : UserIdentityRole
            where TKey : IEquatable<TKey>
            where TUserClaim : UserIdentityUserClaim
            where TUserRole : UserIdentityUserRole
            where TUserLogin : IdentityUserLogin<long>
            where TRoleClaim : UserIdentityRoleClaim
            where TUserToken : UserIdentityUserToken
            where TRoleDto : RoleDto<TRoleDtoKey>
            where TUsersDto : UsersDto<TUserDto, TUserDtoKey>
            where TRolesDto : RolesDto<TRoleDto, TRoleDtoKey>
            where TUserRolesDto : UserRolesDto<TRoleDto, TUserDtoKey, TRoleDtoKey>
            where TUserClaimsDto : UserClaimsDto<TUserDtoKey>
            where TUserProviderDto : UserProviderDto<TUserDtoKey>
            where TUserProvidersDto : UserProvidersDto<TUserDtoKey>
            where TUserChangePasswordDto : UserChangePasswordDto<TUserDtoKey>
            where TRoleClaimsDto : RoleClaimsDto<TRoleDtoKey>
            where TUserClaimDto : UserClaimDto<TUserDtoKey>
            where TRoleClaimDto : RoleClaimDto<TRoleDtoKey>
        {

            return services.AddAdminAspNetIdentityServices<TAdminDbContext, TAdminDbContext, TUserDto, TUserDtoKey, TRoleDto, TRoleDtoKey, TUserKey,
                TRoleKey, TUser, TRole, long, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
                TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
                TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto, TUserClaimDto, TRoleClaimDto>();
        }

        public static IServiceCollection AddAdminAspNetIdentityServices<TIdentityDbContext, TPersistedGrantDbContext, TUserDto, TUserDtoKey, TRoleDto, TRoleDtoKey, TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
                    TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
                    TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto, TUserClaimDto, TRoleClaimDto>(
                        this IServiceCollection services)
            where TPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
            where TIdentityDbContext : IdentityDbContext<UserIdentity, UserIdentityRole, long,
                UserIdentityUserClaim, UserIdentityUserRole, UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken>
            where TUserDto : UserDto<TUserDtoKey>
            where TUser : UserIdentity
            where TRole : UserIdentityRole
            where TKey : IEquatable<TKey>
            where TUserClaim : UserIdentityUserClaim
            where TUserRole : UserIdentityUserRole
            where TUserLogin : IdentityUserLogin<long>
            where TRoleClaim : UserIdentityRoleClaim
            where TUserToken : UserIdentityUserToken
            where TRoleDto : RoleDto<TRoleDtoKey>
            where TUsersDto : UsersDto<TUserDto, TUserDtoKey>
            where TRolesDto : RolesDto<TRoleDto, TRoleDtoKey>
            where TUserRolesDto : UserRolesDto<TRoleDto, TUserDtoKey, TRoleDtoKey>
            where TUserClaimsDto : UserClaimsDto<TUserDtoKey>
            where TUserProviderDto : UserProviderDto<TUserDtoKey>
            where TUserProvidersDto : UserProvidersDto<TUserDtoKey>
            where TUserChangePasswordDto : UserChangePasswordDto<TUserDtoKey>
            where TRoleClaimsDto : RoleClaimsDto<TRoleDtoKey>
            where TUserClaimDto : UserClaimDto<TUserDtoKey>
            where TRoleClaimDto : RoleClaimDto<TRoleDtoKey>
        {
            return AddAdminAspNetIdentityServices<TIdentityDbContext, TPersistedGrantDbContext, TUserDto, TUserDtoKey,
                TRoleDto, TRoleDtoKey, TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin,
                TRoleClaim, TUserToken,
                TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
                TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto, TUserClaimDto,
                TRoleClaimDto>(services, null);
        }

        public static IServiceCollection AddAdminAspNetIdentityServices<TIdentityDbContext, TPersistedGrantDbContext, TUserDto, TUserDtoKey, TRoleDto, TRoleDtoKey, TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
                    TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
                    TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto, TUserClaimDto, TRoleClaimDto>(
                        this IServiceCollection services, HashSet<Type> profileTypes)
            where TPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
            where TIdentityDbContext : IdentityDbContext<UserIdentity, UserIdentityRole, long,
                UserIdentityUserClaim, UserIdentityUserRole, UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken>
            where TUserDto : UserDto<TUserDtoKey>
            where TUser : UserIdentity
            where TRole : UserIdentityRole
            where TKey : IEquatable<TKey>
            where TUserClaim : UserIdentityUserClaim
            where TUserRole : UserIdentityUserRole
            where TUserLogin : IdentityUserLogin<long>
            where TRoleClaim : UserIdentityRoleClaim
            where TUserToken : UserIdentityUserToken
            where TRoleDto : RoleDto<TRoleDtoKey>
            where TUsersDto : UsersDto<TUserDto, TUserDtoKey>
            where TRolesDto : RolesDto<TRoleDto, TRoleDtoKey>
            where TUserRolesDto : UserRolesDto<TRoleDto, TUserDtoKey, TRoleDtoKey>
            where TUserClaimsDto : UserClaimsDto<TUserDtoKey>
            where TUserProviderDto : UserProviderDto<TUserDtoKey>
            where TUserProvidersDto : UserProvidersDto<TUserDtoKey>
            where TUserChangePasswordDto : UserChangePasswordDto<TUserDtoKey>
            where TRoleClaimsDto : RoleClaimsDto<TRoleDtoKey>
            where TUserClaimDto : UserClaimDto<TUserDtoKey>
            where TRoleClaimDto : RoleClaimDto<TRoleDtoKey>
        {
            //Repositories
            services.AddTransient<IIdentityRepository<TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>, IdentityRepository<TIdentityDbContext, TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>>();
            services.AddTransient<IPersistedGrantAspNetIdentityRepository, PersistedGrantAspNetIdentityRepository<TIdentityDbContext, TPersistedGrantDbContext, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>>();
          
            //Services
            services.AddTransient<IIdentityService<TUserDto, TUserDtoKey, TRoleDto, TRoleDtoKey, TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
                TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
                TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto>, 
                IdentityService<TUserDto, TUserDtoKey, TRoleDto, TRoleDtoKey, TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
                    TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
                    TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto>>();
            services.AddTransient<IPersistedGrantAspNetIdentityService, PersistedGrantAspNetIdentityService>();
            
            //Resources
            services.AddScoped<IIdentityServiceResources, IdentityServiceResources>();
            services.AddScoped<IPersistedGrantAspNetIdentityServiceResources, PersistedGrantAspNetIdentityServiceResources>();

            //Register mapping
            services.AddAdminAspNetIdentityMapping()
                .UseIdentityMappingProfile<TUserDto, TUserDtoKey, TRoleDto, TRoleDtoKey, TUser, TRole, long, TUserClaim,
                    TUserRole, TUserLogin, TRoleClaim, TUserToken,
                    TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
                    TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto, TUserClaimDto,
                    TRoleClaimDto>()
                .AddProfilesType(profileTypes);

            return services;
        }
    }
}
