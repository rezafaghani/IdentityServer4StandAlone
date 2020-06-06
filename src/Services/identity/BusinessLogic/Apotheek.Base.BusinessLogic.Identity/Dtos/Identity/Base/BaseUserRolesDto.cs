using Apotheek.Base.BusinessLogic.Identity.Dtos.Identity.Interfaces;

namespace Apotheek.Base.BusinessLogic.Identity.Dtos.Identity.Base
{
    public class BaseUserRolesDto<TUserDtoKey, TRoleDtoKey> : IBaseUserRolesDto
    {
        public TUserDtoKey UserId { get; set; }

        public TRoleDtoKey RoleId { get; set; }

        object IBaseUserRolesDto.UserId => UserId;

        object IBaseUserRolesDto.RoleId => RoleId;
    }
}