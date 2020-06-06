using System.Collections.Generic;

namespace Apotheek.Base.Identity.Api.Dtos.PersistedGrants
{
    public class PersistedGrantSubjectsApiDto
    {
        public PersistedGrantSubjectsApiDto()
        {
            PersistedGrants = new List<PersistedGrantSubjectApiDto>();
        }        

        public int TotalCount { get; set; }

        public int PageSize { get; set; }

        public List<PersistedGrantSubjectApiDto> PersistedGrants { get; set; }
    }
}