namespace Apotheek.Base.Identity.Api.Configuration
{
    public class AdminConfiguration
    {
        public string ApiName { get; set; }

        public string ApiVersion { get; set; }

        public string IdentityServerBaseUrl { get; set; }

        public string ApiBaseUrl { get; set; }

        public string OidcSwaggerUiClientId { get; set; }

        public bool RequireHttpsMetadata { get; set; }

        public string OidcApiName { get; set; }


        public bool CorsAllowAnyOrigin { get; set; }

        public string[] CorsAllowOrigins { get; set; }
        public string IdentityAdminBaseUrl { get; set; }
        public string AdministrationRole { get; set; }

        public string IdentityAdminRedirectUri { get; set; }
        public string[] Scopes { get; set; }
        public string IdentityAdminCookieName { get; set; }
        public double IdentityAdminCookieExpiresUtcHours { get; set; }
        public string TokenValidationClaimName { get; set; }
        public string TokenValidationClaimRole { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string OidcResponseType { get; set; }
        public string JwtTokenIssuer { get; set; }
        public string JwtTokenAudiance { get; set; }
        public string JwtSecurityKey { get; set; }
    }
}