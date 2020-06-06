using System.Threading.Tasks;
using Apotheek.Base.Identity.Api.Dtos.Accounts;
using IdentityModel.Client;

namespace Apotheek.Base.Identity.Api.Helpers
{
    public interface IIdentityServerHelper
    {
        bool IsAccessTokenValidAsync(TokenValidateDto tokenInfo);
        Task<TokenResponse>  GetNewToken(TokenRequest input);
    }
}
