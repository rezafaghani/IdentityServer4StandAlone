using System.Collections.Generic;
using Apotheek.Base.BusinessLogic.Identity.Dtos.Identity;

namespace Apotheek.Base.Identity.Api.Dtos.Users
{
    public class CreateUserDto<TUserDtoKey, TRoleDtoKey>
    {
        public UserDto<TUserDtoKey> User { get; set; }
        public List<UserClaimApiDto<TUserDtoKey>> UserClaim { get; set; }
        public List<UserRoleApiDto<TUserDtoKey, TRoleDtoKey>> UserRole { get; set; }
        public UserChangePasswordApiDto<TUserDtoKey> Password { get; set; }
    }
}
