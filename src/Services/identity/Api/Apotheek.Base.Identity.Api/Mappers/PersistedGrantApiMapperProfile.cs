using Apotheek.Base.BusinessLogic.Dtos.Grant;
using Apotheek.Base.Identity.Api.Dtos.PersistedGrants;
using AutoMapper;

namespace Apotheek.Base.Identity.Api.Mappers
{
    public class PersistedGrantApiMapperProfile : Profile
    {
        public PersistedGrantApiMapperProfile()
        {
            CreateMap<PersistedGrantDto, PersistedGrantApiDto>(MemberList.Destination);
            CreateMap<PersistedGrantDto, PersistedGrantSubjectApiDto>(MemberList.Destination);
            CreateMap<PersistedGrantsDto, PersistedGrantsApiDto>(MemberList.Destination);
        }
    }
}