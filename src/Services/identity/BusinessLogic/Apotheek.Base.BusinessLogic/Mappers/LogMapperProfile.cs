using Apotheek.Base.AuditLogging.EntityFramework.Entities;
using Apotheek.Base.BusinessLogic.Dtos.Log;
using Apotheek.Base.EntityFramework.Entities;
using Apotheek.Base.EntityFramework.Extensions.Common;
using AutoMapper;

namespace Apotheek.Base.BusinessLogic.Mappers
{
    public class LogMapperProfile : Profile
    {
        public LogMapperProfile()
        {
            CreateMap<Log, LogDto>(MemberList.Destination)
                .ReverseMap();
            
            CreateMap<PagedList<Log>, LogsDto>(MemberList.Destination)
                .ForMember(x => x.Logs, opt => opt.MapFrom(src => src.Data));

            CreateMap<AuditLog, AuditLogDto>(MemberList.Destination)
                .ReverseMap();

            CreateMap<PagedList<AuditLog>, AuditLogsDto>(MemberList.Destination)
                .ForMember(x => x.Logs, opt => opt.MapFrom(src => src.Data));
        }
    }
}
