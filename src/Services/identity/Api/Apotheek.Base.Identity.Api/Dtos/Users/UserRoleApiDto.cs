﻿namespace Apotheek.Base.Identity.Api.Dtos.Users
{
    public class UserRoleApiDto<TUserDtoKey, TRoleDtoKey>
    {
        public TUserDtoKey UserId { get; set; }

        public TRoleDtoKey RoleId { get; set; }
    }
}