﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Apotheek.Base.BusinessLogic.Identity.Dtos.Identity;
using Apotheek.Base.EntityFramework.Shared.Entities.Identity;
using Apotheek.Base.Identity.Api.Dtos.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Apotheek.Base.Identity.Api.Configuration.ApplicationParts
{
    public class GenericTypeControllerFeatureProvider<TUserDto, TUserDtoKey, TRoleDto, TCreateUserDto, TRoleDtoKey, TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
        TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
        TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto> : IApplicationFeatureProvider<ControllerFeature>
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
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var currentAssembly = typeof(GenericTypeControllerFeatureProvider<TUserDto, TUserDtoKey, TRoleDto, TCreateUserDto, TRoleDtoKey, TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
                TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
                TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto>).Assembly;
            var controllerTypes = currentAssembly.GetExportedTypes()
                                                 .Where(t => typeof(ControllerBase).IsAssignableFrom(t) && t.IsGenericTypeDefinition)
                                                 .Select(t => t.GetTypeInfo());

            var type = this.GetType();
            var genericType = type.GetGenericTypeDefinition().GetTypeInfo();
            var parameters = genericType.GenericTypeParameters
                                        .Select((p, i) => new { p.Name, Index = i })
                                        .ToDictionary(a => a.Name, a => type.GenericTypeArguments[a.Index]);

            foreach (var controllerType in controllerTypes)
            {
                var typeArguments = controllerType.GenericTypeParameters
                                                  .Select(p => parameters[p.Name])
                                                  .ToArray();

                feature.Controllers.Add(controllerType.MakeGenericType(typeArguments).GetTypeInfo());
            }
        }
    }
}