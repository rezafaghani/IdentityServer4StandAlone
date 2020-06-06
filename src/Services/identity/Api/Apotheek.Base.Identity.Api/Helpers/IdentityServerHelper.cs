using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Threading.Tasks;
using Apotheek.Base.Identity.Api.Dtos.Accounts;
using IdentityModel.Client;

namespace Apotheek.Base.Identity.Api.Helpers
{
    public  class IdentityServerHelper: IIdentityServerHelper
    {
        public bool IsAccessTokenValidAsync(TokenValidateDto tokenInfo)
        {
            if (string.IsNullOrWhiteSpace(tokenInfo.Token))
            {
                return false;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(tokenInfo.Token);

            if (jwtSecurityToken.ValidTo < DateTime.UtcNow.AddSeconds(10))
                return false;
            return true;
            //return response.IsError == false && response.IsActive;
        }

        public async Task<TokenResponse> GetNewToken(TokenRequest input)
        {
            var client = new HttpClient();

            var response = await client.RequestTokenAsync(input);
            return response;
            //if (response.IsError)
            //{
            //    ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            //    return Unauthorized(ModelState);
            //}
        }
    }
}
