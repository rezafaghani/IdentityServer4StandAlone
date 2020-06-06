using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.Administration;
using Ocelot.Authorisation;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;


namespace Apotheek.Base.ApiGateway.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authenticationProviderKey = "ApiAuthKey";

            //IdentityServerValidator -- Well Tested
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(authenticationProviderKey, options =>
            {
                options.Authority = Configuration.GetValue<string>("IdentityServiceAddress");
                // name of the API resource
                //options.Audience = "recipelog";
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters.ValidateAudience = false;
                
            });

            //LocalValidator--Well Tested
            services.AddAuthorization();
            //services.AddAuthentication()
            //                    .AddJwtBearer(authenticationProviderKey, x =>
            //                    {
            //                        x.TokenValidationParameters = new TokenValidationParameters()
            //                        {
            //                            RoleClaimType = "role",
            //                            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration.GetValue<string>("JwtSecurityKey"))),
            //                            ValidIssuer = Configuration.GetValue<string>("JwtTokenIssuer"),
            //                            ValidAudience = Configuration.GetValue<string>("JwtTokenAudiance")
            //                        };
            //                    });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    //.WithOrigins("http://newapigateway.bluebits.ir", "http://newapigateway.bluebits.ir:33080", "http://localhost:3403")
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.AddOcelot()
                .AddDelegatingHandler<DelegatingHandlers.GlobalDelegatingHandler>(true)
                .AddDelegatingHandler<DelegatingHandlers.BezorgtDelegatingHandler>()
                .AddDelegatingHandler<DelegatingHandlers.CoachDelegatingHandler>()
                .AddDelegatingHandler<DelegatingHandlers.PatientDelegatingHandler>()
                .AddDelegatingHandler<DelegatingHandlers.PostalAddressDelegatingHandler>()
                .AddDelegatingHandler<DelegatingHandlers.UserManagementDelegatingHandler>()
                .AddAdministration("/administration", "P@$$W0rD");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseHttpsRedirection();                        

            app.UseAuthorization();
            app.UseCors("CorsPolicy");


            var configuration = new OcelotPipelineConfiguration
            {
                AuthorisationMiddleware = async (ctx, next) =>
                {
                    if (this.Authorize(ctx))
                    {
                        await next.Invoke();
                    }
                    else
                    {
                        ctx.Errors.Add(new UnauthorisedError($"Fail to authorize"));
                    }
                }
            };


            app.UseOcelot(configuration).Wait();
        }

        private bool Authorize(DownstreamContext ctx)
        {
            if (string.IsNullOrEmpty(ctx.DownstreamReRoute.AuthenticationOptions.AuthenticationProviderKey))
            {
                return true;
            }
            else
            {
                //flag for authorization
                bool auth = false;

                //where are stored the claims of the jwt token
                Claim[] claims = ctx.HttpContext.User.Claims.ToArray<Claim>();

                //where are stored the required claims for the route
                Dictionary<string, string> requiredClaims = ctx.DownstreamReRoute.RouteClaimsRequirement;

                foreach (var requiredClaim in requiredClaims)
                {
                    var claimExists = claims.Any(c => c.Type.ToLower() == requiredClaim.Key.ToLower() && requiredClaim.Value.ToLower().Contains(c.Value.ToLower()));
                    if (claimExists)
                    {
                        return true;
                    }
                }

                return auth;
            }
        }
    }
}
