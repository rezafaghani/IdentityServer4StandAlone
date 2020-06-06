using System.Collections.Generic;


namespace Apotheek.Base.Identity.Api.Dtos.Accounts
{
    public class UserInfoDto
    {
        public UserDto UserDto { get; set; }
        public List<PharmacyInfoDto> PharmaciesInfo { get; set; }
    }
}
