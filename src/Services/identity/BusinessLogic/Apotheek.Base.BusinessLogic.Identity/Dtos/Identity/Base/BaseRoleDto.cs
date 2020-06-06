using System.Collections.Generic;
using Apotheek.Base.BusinessLogic.Identity.Dtos.Identity.Interfaces;

namespace Apotheek.Base.BusinessLogic.Identity.Dtos.Identity.Base
{
    public class BaseRoleDto<TRoleId> : IBaseRoleDto
    {
        public TRoleId Id { get; set; }

        public bool IsDefaultId() => EqualityComparer<TRoleId>.Default.Equals(Id, default(TRoleId));

        object IBaseRoleDto.Id => Id;
    }
}