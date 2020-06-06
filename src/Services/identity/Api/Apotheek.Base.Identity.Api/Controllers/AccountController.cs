using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apotheek.Base.BusinessLogic.Identity.Dtos.Identity;
using Apotheek.Base.BusinessLogic.Identity.Services.Interfaces;
using Apotheek.Base.EntityFramework.Shared.Entities.Identity;
using Apotheek.Base.Identity.Api.Configuration;
using Apotheek.Base.Identity.Api.Dtos.Accounts;
using Apotheek.Base.Identity.Api.Dtos.Users;
using Apotheek.Base.Identity.Api.ExceptionHandling;
using Apotheek.Base.Identity.Api.Helpers;
using Apotheek.Base.Identity.Api.Helpers.Consts;
using Apotheek.Base.Identity.Api.Helpers.Localization;
using IdentityModel.Client;
using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json;
using Serilog;

namespace Apotheek.Base.Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ControllerExceptionFilterAttribute))]
    [Produces("application/json", "application/problem+json")]
    [Authorize()]
    public class AccountController<TUserDto, TUserDtoKey, TRoleDto, TCreateUserDto, TRoleDtoKey, TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
        TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
        TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto> : ControllerBase
        where TUserDto : UserDto<TUserDtoKey>, new()
        where TRoleDto : RoleDto<TRoleDtoKey>, new()
        where TCreateUserDto : CreateUserDto<TUserDtoKey, TRoleDtoKey>, new()
        where TUser : UserIdentity
        where TRole : UserIdentityRole
        where TKey : IEquatable<TKey>
        where TUserClaim : UserIdentityUserClaim
        where TUserRole : UserIdentityUserRole
        where TUserLogin : IdentityUserLogin<long>
        where TRoleClaim : UserIdentityRoleClaim
        where TUserToken : UserIdentityUserToken
        where TRoleDtoKey : IEquatable<TRoleDtoKey>
        where TUserDtoKey : IEquatable<TUserDtoKey>
        where TUsersDto : UsersDto<TUserDto, TUserDtoKey>
        where TRolesDto : RolesDto<TRoleDto, TRoleDtoKey>
        where TUserRolesDto : UserRolesDto<TRoleDto, TUserDtoKey, TRoleDtoKey>
        where TUserClaimsDto : UserClaimsDto<TUserDtoKey>, new()
        where TUserProviderDto : UserProviderDto<TUserDtoKey>
        where TUserProvidersDto : UserProvidersDto<TUserDtoKey>
        where TUserChangePasswordDto : UserChangePasswordDto<TUserDtoKey>
        where TRoleClaimsDto : RoleClaimsDto<TRoleDtoKey>
    {
        private readonly IIdentityService<TUserDto, TUserDtoKey, TRoleDto, TRoleDtoKey, TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
            TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
            TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto> _identityService;
        private readonly UserResolver<TUser> _userResolver;
        private readonly UserManager<TUser> _userManager;
        private readonly SignInManager<TUser> _signInManager;
        private readonly IClientStore _clientStore;
        private readonly IIdentityServerHelper _identityServerHelper;
        private readonly IEventService _events;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        private readonly IGenericControllerLocalizer<AccountController<TUserDto, TUserDtoKey, TRoleDto, TCreateUserDto, TRoleDtoKey, TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
            TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
            TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto>> _localizer;

        private readonly AdminConfiguration _adminConfiguration;


        public AccountController(UserResolver<TUser> userResolver, UserManager<TUser> userManager, SignInManager<TUser> signInManager, IClientStore clientStore, IEventService events, IEmailSender emailSender, IGenericControllerLocalizer<AccountController<TUserDto, TUserDtoKey, TRoleDto, TCreateUserDto, TRoleDtoKey, TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
            TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
            TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto>> localizer, AdminConfiguration adminConfiguration, IIdentityService<TUserDto, TUserDtoKey, TRoleDto, TRoleDtoKey, TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken, TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto, TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto> identityService, IIdentityServerHelper identityServerHelper, ILogger logger)
        {
            _userResolver = userResolver;
            _userManager = userManager;
            _signInManager = signInManager;
            _clientStore = clientStore;
            _events = events;
            _emailSender = emailSender;
            _localizer = localizer;
            _adminConfiguration = adminConfiguration;
            _identityService = identityService;
            _identityServerHelper = identityServerHelper;
            _logger = logger;
        }
        /// <summary>
        /// Handle postback from username/password login
        /// </summary>

        [HttpPost("login")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginInputModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userResolver.GetUserAsync(model.Username);
                if (user == null)
                    return BadRequest($"User {model.Username} is not exist");
                //var user = await _identityService.GetUserAsync(Convert.ToString(userDto.Id));
                var userRoles = await _identityService.GetUserRolesAsync(user.Id.ToString());
                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberLogin, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.UserName));


                }

                if (result.IsLockedOut)
                {
                    return new JsonResult("Lockout");
                }

                var token = await _userManager.GetAuthenticationTokenAsync(user, "oidc", "TokenName");
                var isValidToken = _identityServerHelper.IsAccessTokenValidAsync(new TokenValidateDto
                {
                    Token = token
                });

                var identityClient = await _clientStore.FindClientByIdAsync(ClientConst.ClientId);
                if (identityClient == null)
                    return BadRequest("Client id is not valid");
                if (string.IsNullOrWhiteSpace(token))
                {

                    var response = await _identityServerHelper.GetNewToken(new TokenRequest
                    {
                        Address = $"{_adminConfiguration.IdentityServerBaseUrl}/connect/token",
                        GrantType = "password",
                        ClientId = identityClient.ClientId,
                        //ClientSecret = _adminConfiguration.ClientSecret,
                        Parameters =
                            {
                                {"username", model.Username},
                                {"password", model.Password},
                                {"scope", ClientConst.Scope}
                            }
                    });
                    if (response.IsError)
                    {
                        ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
                        return BadRequest(Unauthorized(ModelState));
                    }
                    token = response.AccessToken;
                }

                if (!isValidToken)
                {


                    await _userManager.RemoveAuthenticationTokenAsync(user, "oidc", "TokenName");
                    var response = await _identityServerHelper.GetNewToken(new TokenRequest
                    {
                        Address = $"{_adminConfiguration.IdentityServerBaseUrl}/connect/token",
                        GrantType = "password",
                        ClientId = identityClient.ClientId,
                        //ClientSecret = _adminConfiguration.ClientSecret,
                        Parameters =
                            {
                                {"username", model.Username},
                                {"password", model.Password},
                                {"scope", ClientConst.Scope}
                            }
                    });
                    if (response.IsError)
                    {
                        ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
                        return BadRequest(Unauthorized(ModelState));
                    }
                    token = response.AccessToken;
                }

                await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.UserName));


                await _userManager.SetAuthenticationTokenAsync(user, "oidc", "TokenName", token);

                if (result.RequiresTwoFactor)
                {

                    return new JsonResult(new LoginResultDto
                    {
                        Token = "",
                        NextStepNeeded = user.TwoFactorEnabled,
                        UserInfo = new UserInfoDto
                        {
                            PharmaciesInfo = new List<PharmacyInfoDto>
                            {
                                new PharmacyInfoDto
                                {
                                    AGB = "02009717",
                                    CustomerId = user.CustomerId,
                                    Id = user.PharmacyId,
                                    IsDepot = true,
                                    Name ="Apotheek de Duffelt"
                                }
                            },
                            UserDto = new UserDto
                            {
                                Id = Convert.ToInt64(user.Id.ToString()),
                                Name = user.UserName,
                                Email = user.Email,
                                Initials = "",
                                IsActive = !user.LockoutEnabled,
                                IsTwoFaEnabled = user.TwoFactorEnabled,
                                PhoneNumber = user.PhoneNumber,
                                Username = user.UserName,
                                Roles = userRoles != null ? userRoles.Roles.Select(x => new RoleDto
                                {
                                    Id = Convert.ToInt64(x.Id.ToString()),
                                    Title = x.Name,
                                    Code = x.Id.ToString()
                                }) : new List<RoleDto>()
                            }
                        }
                    });
                }
                _logger.Information($"user {user.UserName} log in {DateTime.Now}");
                return new JsonResult(new LoginResultDto
                {
                    Token = token,
                    NextStepNeeded = user.TwoFactorEnabled,
                    UserInfo = new UserInfoDto
                    {
                        PharmaciesInfo = new List<PharmacyInfoDto>
                            {
                                new PharmacyInfoDto
                                {
                                    AGB = "02009717",
                                    CustomerId = user.CustomerId,
                                    Id = user.PharmacyId,
                                    IsDepot = true,
                                    Name ="Apotheek de Duffelt"
                                }
                            },
                        UserDto = new UserDto
                        {
                            Id = Convert.ToInt64(user.Id.ToString()),
                            Name = user.UserName,
                            Email = user.Email,
                            Initials = "",
                            IsActive = !user.LockoutEnabled,
                            IsTwoFaEnabled = user.TwoFactorEnabled,
                            PhoneNumber = user.PhoneNumber,
                            Username = user.UserName,
                            Roles = userRoles!=null? userRoles.Roles.Select(x=>new RoleDto
                            {
                                Id = Convert.ToInt64(x.Id.ToString()),
                                Title = x.Name,
                                Code = x.Id.ToString()
                            }):new List<RoleDto>()
                        }
                    }
                });
                
            }
            await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials"));
            ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            return Unauthorized(ModelState);
        }

        [HttpPost("LogOut")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [AllowAnonymous]
        public async Task<IActionResult> LogOut([FromBody] LogoutModelDto model)
        {
            var userDto = await _userResolver.GetUserAsync(model.Username);
            if (userDto != null)
            {
                var token = await _userManager.GetAuthenticationTokenAsync(userDto, "oidc", "TokenName");
                if (string.IsNullOrWhiteSpace(token))
                    return Ok();
                var result = await _userManager.RemoveAuthenticationTokenAsync(userDto, "oidc", "TokenName");
                if (result.Succeeded)
                    return Ok();
                return BadRequest(result);
            }

            return BadRequest($"User {model.Username} is not exist");
        }

        [HttpPost("LoginWith2Fa")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2Fa([FromBody] LoginWith2FaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, _localizer["LoginWith2faError"]);
                Unauthorized(ModelState);
                return new JsonResult(ModelState);

            }

            var user = await _userResolver.GetUserAsync(model.Username);
            if (user == null)
            {
                throw new InvalidOperationException(_localizer["Unable2FA"]);
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result2 = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider,
                  authenticatorCode);
            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, model.RememberMe, model.RememberMachine);

            if (result2)
            {
                var userRoles = await _identityService.GetUserRolesAsync(user.Id.ToString());
                var token = await _userManager.GetAuthenticationTokenAsync(user, "oidc", "TokenName");
                return new JsonResult(new LoginResultDto
                {
                    Token = token,
                    NextStepNeeded = false,
                    UserInfo = new UserInfoDto
                    {
                        PharmaciesInfo = new List<PharmacyInfoDto>
                        {
                            new PharmacyInfoDto
                            {
                                AGB = "02009717",
                                CustomerId = user.CustomerId,
                                Id = user.PharmacyId,
                                IsDepot = true,
                                Name ="Apotheek de Duffelt"
                            }
                        },
                        UserDto = new UserDto
                        {
                            Id = Convert.ToInt64(user.Id.ToString()),
                            Name = user.UserName,
                            Email = user.Email,
                            Initials = "",
                            IsActive = !user.LockoutEnabled,
                            IsTwoFaEnabled = user.TwoFactorEnabled,
                            PhoneNumber = user.PhoneNumber,
                            Username = user.UserName,
                            Roles = userRoles.Roles != null ? userRoles.Roles.Select(x => new RoleDto
                            {
                                Id = Convert.ToInt64(x.Id.ToString()),
                                Title = x.Name,
                                Code = x.Id.ToString()
                            }) : new List<RoleDto>()
                        }
                    }
                });
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, _localizer["Lockout"]);
                Unauthorized(ModelState);
                return new JsonResult(ModelState);

            }

            ModelState.AddModelError(string.Empty, _localizer["InvalidAuthenticatorCode"]);
            Unauthorized(ModelState);
            return BadRequest(ModelState);
        }
        [HttpPost("ResetPassword")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return new JsonResult("User Not Found");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return new JsonResult(result);
            }

            AddErrors(result);

            return new JsonResult(result);
        }
        [HttpPost("Register")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {

            if (!ModelState.IsValid) return new JsonResult(model);

            var user = new UserIdentity
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync((TUser)user, model.Password);
            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync((TUser)user);
                //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code }, HttpContext.Request.Scheme);
                var confirmResult = await _userManager.ConfirmEmailAsync((TUser)user, code);
                await _emailSender.SendEmailAsync(model.Email, _localizer["ConfirmEmailTitle"], _localizer["ConfirmEmailBody", _localizer["ConfirmMessage"]]);
                await _signInManager.SignInAsync((TUser)user, isPersistent: false);

                return new JsonResult(result);
            }

            AddErrors(result);

            // If we got this far, something failed, redisplay form
            return new JsonResult(model);
        }

        [HttpPost("RegisterWithoutUsername")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterWithoutUsername([FromBody] RegisterWithoutUsernameViewModel model)
        {
            var registerModel = new RegisterViewModel
            {
                UserName = model.Email,
                Email = model.Email,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword
            };

            return await Register(registerModel);
        }
        [HttpPost("SwitchUser")]
        [Authorize]
        public async Task<IActionResult> SwitchUser([FromBody] SwitchUserDto model)
        {
            var currentUserDto = await _userManager.FindByNameAsync(model.CurrentUserName);
            var newUserDto = await _userManager.FindByNameAsync(model.NewUserName);
            if (currentUserDto == null)
            {
                return BadRequest("Current user is invalid");
            }
            if (newUserDto == null)
            {
                return BadRequest("newUserDto user is invalid");
            }

            if (currentUserDto.CustomerId != newUserDto.CustomerId)
            {
                return BadRequest("this user is not for this customer");
            }

            if (newUserDto.PinCode != model.PinCode)
                return BadRequest("The pinCode is not valid");
            var token = await _userManager.GetAuthenticationTokenAsync(newUserDto, "oidc", "TokenName");
            if (string.IsNullOrWhiteSpace(token))
                return BadRequest("User must be login before");
            var isTokenValid = _identityServerHelper.IsAccessTokenValidAsync(new TokenValidateDto
            {
                Token = token

            });


            if (!isTokenValid)
            {
                await _userManager.RemoveAuthenticationTokenAsync(newUserDto, "oidc", "TokenName");
                return BadRequest("User login is expired");
            }

            return new JsonResult(new
            {
                JwtToken = token,
                UserInfo = newUserDto

            });
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
