using System;
using System.Collections.Generic;
using Apotheek.Base.BusinessLogic.Identity.Dtos.Identity;
using Apotheek.Base.EntityFramework.Shared.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Apotheek.Base.BusinessLogic.Identity.Mappers.Configuration
{
    public class MapperConfigurationBuilder : IMapperConfigurationBuilder
    {
        public HashSet<Type> ProfileTypes { get; } = new HashSet<Type>();

        public IMapperConfigurationBuilder AddProfilesType(HashSet<Type> profileTypes)
        {
            if (profileTypes == null) return this;

            foreach (var profileType in profileTypes)
            {
                ProfileTypes.Add(profileType);
            }

            return this;
        }

        public IMapperConfigurationBuilder UseIdentityMappingProfile<TUserDto, TUserDtoKey, TRoleDto, TRoleDtoKey, TUser, TRole, TKey,
            TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
            TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
            TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto,
            TUserClaimDto, TRoleClaimDto>()
            where TUserDto : UserDto<TUserDtoKey>
            where TRoleDto : RoleDto<TRoleDtoKey>
            where TUser : UserIdentity
            where TRole : UserIdentityRole
            where TKey : IEquatable<TKey>
            where TUserClaim : UserIdentityUserClaim
            where TUserRole : UserIdentityUserRole
            where TUserLogin : IdentityUserLogin<long>
            where TRoleClaim : UserIdentityRoleClaim
            where TUserToken : UserIdentityUserToken
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
            ProfileTypes.Add(typeof(IdentityMapperProfile<TUserDto, TUserDtoKey, TRoleDto, TRoleDtoKey, TUser, TRole, TKey,
                TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
                TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
                TUserProviderDto, TUserProvidersDto, TRoleClaimsDto, TUserClaimDto, TRoleClaimDto>));

            return this;
        }
    }
}