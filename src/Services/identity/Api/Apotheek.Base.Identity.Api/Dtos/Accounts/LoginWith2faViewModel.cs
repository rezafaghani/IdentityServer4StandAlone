using System.ComponentModel.DataAnnotations;

namespace Apotheek.Base.Identity.Api.Dtos.Accounts
{
    public class LoginWith2FaViewModel
    {
        [Required]
        [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        public string TwoFactorCode { get; set; }
        public bool RememberMachine { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
        public string Username { get; set; }
        //public string ClientId { get; set; }

        //public string Password { get; set; }
        //public string Scope { get; set; }
    }
}
