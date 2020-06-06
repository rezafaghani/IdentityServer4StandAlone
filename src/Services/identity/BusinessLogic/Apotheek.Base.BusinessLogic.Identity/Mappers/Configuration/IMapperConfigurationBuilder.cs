using System;
using System.Collections.Generic;
using Apotheek.Base.BusinessLogic.Identity.Dtos.Identity;
using Apotheek.Base.EntityFramework.Shared.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Apotheek.Base.BusinessLogic.Identity.Mappers.Configuration
{
    public interface IMapperConfigurationBuilder
    {
        HashSet<Type> ProfileTypes { get; }

        IMapperConfigurationBuilder AddProfilesType(HashSet<Type> profileTypes);

        IMapperConfigurationBuilder UseIdentityMappingProfile<TUserDto, TUserDtoKey, TRoleDto, TRoleDtoKey, TUser, TRole,
            TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
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
            where TRoleClaimDto : RoleClaimDto<TRoleDtoKey>;
    }
}
