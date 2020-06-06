namespace Apotheek.Base.Identity.Api.Dtos.Accounts
{
    public class LoginInputModel
    {

        public string Username { get; set; }
        
        public string Password { get; set; }
        
        public bool RememberLogin { get; set; }
        
    }
}
