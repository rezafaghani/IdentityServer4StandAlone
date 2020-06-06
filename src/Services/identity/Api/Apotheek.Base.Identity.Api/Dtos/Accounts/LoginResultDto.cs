using Apotheek.Base.BusinessLogic.Identity.Dtos.Identity;

namespace Apotheek.Base.Identity.Api.Dtos.Accounts
{
    public class LoginResultDto
    {
        public string Token { get; set; }
        public bool NextStepNeeded { get; set; }
        public UserInfoDto UserInfo { get; set; }
    }
}
