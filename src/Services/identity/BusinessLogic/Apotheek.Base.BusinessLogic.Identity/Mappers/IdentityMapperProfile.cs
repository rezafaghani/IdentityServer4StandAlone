using System;
using System.Collections.Generic;
using Apotheek.Base.BusinessLogic.Identity.Dtos.Identity;
using Apotheek.Base.BusinessLogic.Shared.ExceptionHandling;
using Apotheek.Base.EntityFramework.Extensions.Common;
using Apotheek.Base.EntityFramework.Shared.Entities.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Apotheek.Base.BusinessLogic.Identity.Mappers
{
    public class IdentityMapperProfile<TUserDto, TUserDtoKey, TRoleDto, TRoleDtoKey, TUser, TRole, TKey, TUserClaim, TUserRole,
        TUserLogin, TRoleClaim, TUserToken,
        TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
        TUserProviderDto, TUserProvidersDto, TRoleClaimsDto,
        TUserClaimDto, TRoleClaimDto>
        : Profile
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
        where TRoleClaimsDto : RoleClaimsDto<TRoleDtoKey>
        where TUserClaimDto : UserClaimDto<TUserDtoKey>
        where TRoleClaimDto : RoleClaimDto<TRoleDtoKey>
    {
        public IdentityMapperProfile()
        {
            // entity to model
            CreateMap<TUser, TUserDto>(MemberList.Destination);

            CreateMap<UserLoginInfo, TUserProviderDto>(MemberList.Destination);

            CreateMap<IdentityError, ViewErrorMessage>(MemberList.Destination)
                .ForMember(x => x.ErrorKey, opt => opt.MapFrom(src => src.Code))
                .ForMember(x => x.ErrorMessage, opt => opt.MapFrom(src => src.Description));

            // entity to model
            CreateMap<TRole, TRoleDto>(MemberList.Destination);

            CreateMap<TUser, TUser>(MemberList.Destination)
                .ForMember(x => x.SecurityStamp, opt => opt.Ignore());

            CreateMap<PagedList<TUser>, TUsersDto>(MemberList.Destination)
                .ForMember(x => x.Users,
                    opt => opt.MapFrom(src => src.Data));

            CreateMap<TUserClaim, TUserClaimDto>(MemberList.Destination)
                .ForMember(x => x.ClaimId, opt => opt.MapFrom(src => src.Id));

            CreateMap<TUserClaim, TUserClaimsDto>(MemberList.Destination)
                .ForMember(x => x.ClaimId, opt => opt.MapFrom(src => src.Id));

            CreateMap<PagedList<TRole>, TRolesDto>(MemberList.Destination)
                .ForMember(x => x.Roles,
                    opt => opt.MapFrom(src => src.Data));

            CreateMap<PagedList<TRole>, TUserRolesDto>(MemberList.Destination)
                .ForMember(x => x.Roles,
                    opt => opt.MapFrom(src => src.Data));

            CreateMap<PagedList<TUserClaim>, TUserClaimsDto>(MemberList.Destination)
                .ForMember(x => x.Claims,
                    opt => opt.MapFrom(src => src.Data));

            CreateMap<PagedList<TRoleClaim>, TRoleClaimsDto>(MemberList.Destination)
                .ForMember(x => x.Claims,
                    opt => opt.MapFrom(src => src.Data));

            CreateMap<List<UserLoginInfo>, TUserProvidersDto>(MemberList.Destination)
                .ForMember(x => x.Providers, opt => opt.MapFrom(src => src));

            CreateMap<TRoleClaim, TRoleClaimDto>(MemberList.Destination)
                .ForMember(x => x.ClaimId, opt => opt.MapFrom(src => src.Id));

            CreateMap<TRoleClaim, TRoleClaimsDto>(MemberList.Destination)
                .ForMember(x => x.ClaimId, opt => opt.MapFrom(src => src.Id));

            CreateMap<TUserLogin, TUserProviderDto>(MemberList.Destination);

            // model to entity
            CreateMap<TRoleDto, TRole>(MemberList.Source)
                .ForMember(dest => dest.Id, opt => opt.Condition(srs => srs.Id != null)); 

            CreateMap<TRoleClaimsDto, TRoleClaim>(MemberList.Source);

            CreateMap<TUserClaimsDto, TUserClaim>(MemberList.Source)
                .ForMember(x => x.Id,
                    opt => opt.MapFrom(src => src.ClaimId));

            // model to entity
            CreateMap<TUserDto, TUser>(MemberList.Source)
                .ForMember(dest => dest.Id, opt => opt.Condition(srs => srs.Id != null)); 
        }
    }
}