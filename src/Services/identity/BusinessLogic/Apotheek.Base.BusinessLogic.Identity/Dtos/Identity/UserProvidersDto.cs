using System.Collections.Generic;
using System.Linq;
using Apotheek.Base.BusinessLogic.Identity.Dtos.Identity.Interfaces;

namespace Apotheek.Base.BusinessLogic.Identity.Dtos.Identity
{
    public class UserProvidersDto<TUserDtoKey> : UserProviderDto<TUserDtoKey>, IUserProvidersDto
    {
        public UserProvidersDto()
        {
            Providers = new List<UserProviderDto<TUserDtoKey>>();
        }

        public List<UserProviderDto<TUserDtoKey>> Providers { get; set; }

        List<IUserProviderDto> IUserProvidersDto.Providers => Providers.Cast<IUserProviderDto>().ToList();
    }
}
