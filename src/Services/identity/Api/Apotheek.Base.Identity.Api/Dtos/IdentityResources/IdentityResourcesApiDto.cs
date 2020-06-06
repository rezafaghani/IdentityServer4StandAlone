using System.Collections.Generic;

namespace Apotheek.Base.Identity.Api.Dtos.IdentityResources
{
    public class IdentityResourcesApiDto
    {
        public IdentityResourcesApiDto()
        {
            IdentityResources = new List<IdentityResourceApiDto>();
        }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public List<IdentityResourceApiDto> IdentityResources { get; set; }
    }
}