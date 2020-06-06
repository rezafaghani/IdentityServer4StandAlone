namespace Apotheek.Base.Identity.Api.Dtos.Accounts
{
    public class SwitchUserDto
    {
        public string CurrentUserName { get; set; }
        public string NewUserName { get; set; }
        public int PinCode { get; set; }
    }
}
