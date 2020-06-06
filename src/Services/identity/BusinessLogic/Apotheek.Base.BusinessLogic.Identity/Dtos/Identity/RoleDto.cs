using System.ComponentModel.DataAnnotations;
using Apotheek.Base.BusinessLogic.Identity.Dtos.Identity.Base;
using Apotheek.Base.BusinessLogic.Identity.Dtos.Identity.Interfaces;

namespace Apotheek.Base.BusinessLogic.Identity.Dtos.Identity
{
    public class RoleDto<TKey> : BaseRoleDto<TKey>, IRoleDto
    {      
        [Required]
        public string Name { get; set; }
    }
}