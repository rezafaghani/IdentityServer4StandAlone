using System.Collections.Generic;

namespace Apotheek.Base.Identity.Api.Dtos.Roles
{
    public class RoleClaimsApiDto<TRoleDtoKey>
    {
        public RoleClaimsApiDto()
        {
            Claims = new List<RoleClaimApiDto<TRoleDtoKey>>();
        }

        public List<RoleClaimApiDto<TRoleDtoKey>> Claims { get; set; }

        public int TotalCount { get; set; }

        public int PageSize { get; set; }
    }
}