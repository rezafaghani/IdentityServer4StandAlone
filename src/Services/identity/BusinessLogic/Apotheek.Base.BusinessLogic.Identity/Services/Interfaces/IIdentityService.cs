using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Apotheek.Base.BusinessLogic.Identity.Dtos.Identity;
using Apotheek.Base.EntityFramework.Shared.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Apotheek.Base.BusinessLogic.Identity.Services.Interfaces
{
    public interface IIdentityService<TUserDto, TUserDtoKey, TRoleDto, TRoleDtoKey, TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, 
        TUserLogin, TRoleClaim, TUserToken, 
        TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
        TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto>        
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
        where TUserClaimsDto: UserClaimsDto<TUserDtoKey>
        where TUserProviderDto : UserProviderDto<TUserDtoKey>
        where TUserProvidersDto : UserProvidersDto<TUserDtoKey>
        where TUserChangePasswordDto: UserChangePasswordDto<TUserDtoKey>
        where TRoleClaimsDto : RoleClaimsDto<TRoleDtoKey>
    {
        Task<bool> ExistsUserAsync(string userId);

        Task<bool> ExistsRoleAsync(string roleId);

        Task<TUsersDto> GetUsersAsync(string search, int page = 1, int pageSize = 10);
        Task<TUsersDto> GetLoginPharmacyUsersAsync(int pharmacyId, string search, int page = 1, int pageSize = 10);
        Task<TUsersDto> GetPharmacyUsersAsync(int pharmacyId, string search, int page = 1, int pageSize = 10);
        Task<TUsersDto> GetRoleUsersAsync(string roleId, string search, int page = 1, int pageSize = 10);
        Task<TRolesDto> GetRolesAsync(string search, int page = 1, int pageSize = 10);

        Task<(IdentityResult identityResult, string roleId)> CreateRoleAsync(TRoleDto role);

        Task<TRoleDto> GetRoleAsync(string roleId);

        Task<List<TRoleDto>> GetRolesAsync();

        Task<(IdentityResult identityResult, string roleId)> UpdateRoleAsync(TRoleDto role);

        Task<TUserDto> GetUserAsync(string userId);
       
        Task<(IdentityResult identityResult, string userId)> CreateUserAsync(TUserDto user);

        Task<(IdentityResult identityResult, string userId)> UpdateUserAsync(TUserDto user);

        Task<IdentityResult> DeleteUserAsync(string userId, TUserDto user);

        Task<IdentityResult> CreateUserRoleAsync(TUserRolesDto role);

        Task<TUserRolesDto> BuildUserRolesViewModel(TUserDtoKey id, int? page);

        Task<TUserRolesDto> GetUserRolesAsync(string userId, int page = 1,
            int pageSize = 10);

        Task<IdentityResult> DeleteUserRoleAsync(TUserRolesDto role);

        Task<TUserClaimsDto> GetUserClaimsAsync(string userId, int page = 1,
            int pageSize = 10);

        Task<TUserClaimsDto> GetUserClaimAsync(string userId, int claimId);

        Task<IdentityResult> CreateUserClaimsAsync(TUserClaimsDto claimsDto);

        Task<int> DeleteUserClaimsAsync(TUserClaimsDto claim);

        Task<TUserProvidersDto> GetUserProvidersAsync(string userId);

        TUserDtoKey ConvertUserDtoKeyFromString(string id);

        Task<IdentityResult> DeleteUserProvidersAsync(TUserProviderDto provider);

        Task<TUserProviderDto> GetUserProviderAsync(string userId, string providerKey);

        Task<IdentityResult> UserChangePasswordAsync(TUserChangePasswordDto userPassword);

        Task<IdentityResult> CreateRoleClaimsAsync(TRoleClaimsDto claimsDto);

        Task<TRoleClaimsDto> GetRoleClaimsAsync(string roleId, int page = 1, int pageSize = 10);

		Task<TRoleClaimsDto> GetUserRoleClaimsAsync(string userId, string claimSearchText, int page = 1, int pageSize = 10);

		Task<TRoleClaimsDto> GetRoleClaimAsync(string roleId, int claimId);

        Task<int> DeleteRoleClaimsAsync(TRoleClaimsDto role);

        Task<IdentityResult> DeleteRoleAsync(TRoleDto role);
    }
}