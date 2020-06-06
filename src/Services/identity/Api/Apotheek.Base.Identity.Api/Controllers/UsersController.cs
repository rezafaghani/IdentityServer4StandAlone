using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Apotheek.Base.Identity.Api.Dtos.Roles;
using Apotheek.Base.Identity.Api.Dtos.Users;
using Apotheek.Base.Identity.Api.ExceptionHandling;
using Apotheek.Base.Identity.Api.Helpers.Localization;
using Apotheek.Base.Identity.Api.Resources;
using Apotheek.Base.BusinessLogic.Identity.Dtos.Identity;
using Apotheek.Base.BusinessLogic.Identity.Services.Interfaces;
using Apotheek.Base.EntityFramework.Shared.Entities.Identity;
using Apotheek.Base.Identity.Api.Helpers;
using AutoMapper;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Apotheek.Base.Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ControllerExceptionFilterAttribute))]
    [Produces("application/json", "application/problem+json")]
    [Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController<TUserDto, TUserDtoKey, TRoleDto, TCreateUserDto, TRoleDtoKey, TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
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
        private readonly IGenericControllerLocalizer<UsersController<TUserDto, TUserDtoKey, TRoleDto, TCreateUserDto, TRoleDtoKey, TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
            TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
            TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto>> _localizer;

        private readonly IMapper _mapper;
        private readonly UserManager<TUser> _userManager;
        private readonly SignInManager<TUser> _signInManager;
        private readonly UrlEncoder _urlEncoder;
        private readonly UserResolver<TUser> _userResolver;
        private readonly ILogger _logger;
        private readonly IApiErrorResources _errorResources;

        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        [TempData]
        public string StatusMessage { get; set; }
        public UsersController(IIdentityService<TUserDto, TUserDtoKey, TRoleDto, TRoleDtoKey, TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
                TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
                TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto> identityService,
            IGenericControllerLocalizer<UsersController<TUserDto, TUserDtoKey, TRoleDto, TCreateUserDto, TRoleDtoKey, TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
                TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
                TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto>> localizer, IMapper mapper, IApiErrorResources errorResources, UserManager<TUser> userManager, ILogger logger, UrlEncoder urlEncoder, SignInManager<TUser> signInManager, UserResolver<TUser> userResolver)
        {
            _identityService = identityService;
            _localizer = localizer;
            _mapper = mapper;
            _errorResources = errorResources;
            _userManager = userManager;
            _logger = logger;
            _urlEncoder = urlEncoder;
            _signInManager = signInManager;
            _userResolver = userResolver;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TUserDto>> Get(TUserDtoKey id)
        {
            var user = await _identityService.GetUserAsync(id.ToString());

            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<TUsersDto>> Get(string searchText, int page = 1, int pageSize = 10)
        {
            var usersDto = await _identityService.GetUsersAsync(searchText, page, pageSize);

            return Ok(usersDto);
        }
        
        [HttpGet("GetLoginPharmacyUsers")]
        public async Task<ActionResult<TUsersDto>> GetLoginPharmacyUsersAsync(int pharmacyId, string searchText, int page = 1, int pageSize = 10)
        {
            var usersDto = await _identityService.GetLoginPharmacyUsersAsync(pharmacyId, searchText, page, pageSize);

            return Ok(usersDto);
        }
        
        [HttpGet("GetPharmacyUsers")]
        public async Task<ActionResult<TUsersDto>> GetPharmacyUsersAsync(int pharmacyId, string searchText, int page = 1, int pageSize = 10)
        {
            var usersDto = await _identityService.GetPharmacyUsersAsync(pharmacyId, searchText, page, pageSize);

            return Ok(usersDto);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<TUserDto>> Post([FromBody] TUserDto user)
        {

            if (!EqualityComparer<TUserDtoKey>.Default.Equals(user.Id, default))
            {
                return BadRequest(_errorResources.CannotSetId());
            }

            var (identityResult, userId) = await _identityService.CreateUserAsync(user);
            var createdUser = await _identityService.GetUserAsync(userId);

            return CreatedAtAction(nameof(Get), new { id = createdUser.Id }, createdUser);
        }

        [HttpPost("CreateUser")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<TUserDto>> CreateUser([FromBody] TCreateUserDto user)
        {
            if (!EqualityComparer<TUserDtoKey>.Default.Equals(user.User.Id, default))
            {
                return BadRequest(_errorResources.CannotSetId());
            }

            var (identityResult, userId) = await _identityService.CreateUserAsync(new TUserDto
            {
                AccessFailedCount = user.User.AccessFailedCount,
                Cluster = user.User.Cluster,
                CustomerId = user.User.CustomerId,
                PharmacyId = user.User.PharmacyId,
                Email = user.User.Email,
                EmailConfirmed = user.User.EmailConfirmed,
                LockoutEnabled = user.User.LockoutEnabled,
                LockoutEnd = user.User.LockoutEnd,
                PhoneNumber = user.User.PhoneNumber,
                PhoneNumberConfirmed = user.User.PhoneNumberConfirmed,
                PinCode = user.User.PinCode,
                TwoFactorEnabled = user.User.TwoFactorEnabled,
                UserName = user.User.UserName
            });
            if (!identityResult.Succeeded)
            {
                return BadRequest(identityResult.Errors);

            }
            var createdUser = await _identityService.GetUserAsync(userId);

            if (user.UserClaim != null && user.UserClaim.Any())
            {
                foreach (var item in user.UserClaim)
                {
                    var userClaimDto = _mapper.Map<TUserClaimsDto>(item);

                    if (userClaimDto.ClaimId.Equals(default))
                    {
                        userClaimDto.UserId = createdUser.Id;
                        await _identityService.CreateUserClaimsAsync(userClaimDto);
                    }
                }
            }

            if (user.UserRole != null && user.UserRole.Any())
            {
                foreach (var item in user.UserRole)
                {
                    if (!string.IsNullOrWhiteSpace(item.RoleId.ToString()))
                    {
                        var userRolesDto = _mapper.Map<TUserRolesDto>(item);
                        userRolesDto.UserId = createdUser.Id;
                        await _identityService.CreateUserRoleAsync(userRolesDto);
                    }
                }
               
            }


            if (user.Password != null)
            {
                if (!string.IsNullOrWhiteSpace(user.Password.Password) && !string.IsNullOrWhiteSpace(user.Password.ConfirmPassword) &&
                    user.Password.Password.Equals(user.Password.ConfirmPassword))
                {
                    user.Password.UserId = createdUser.Id;
                    var userChangePasswordDto = _mapper.Map<TUserChangePasswordDto>(user.Password);

                    await _identityService.UserChangePasswordAsync(userChangePasswordDto);
                }
            }
           

            return CreatedAtAction(nameof(Get), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] TUserDto user)
        {
            await _identityService.GetUserAsync(user.Id.ToString());
            await _identityService.UpdateUserAsync(user);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(TUserDtoKey id)
        {
            var currentUserId = User.GetSubjectId();
            if (id.ToString() == currentUserId)
                return StatusCode((int)System.Net.HttpStatusCode.Forbidden);

            var user = new TUserDto { Id = id };

            await _identityService.GetUserAsync(user.Id.ToString());
            await _identityService.DeleteUserAsync(user.Id.ToString(), user);

            return Ok();
        }

        [HttpGet("{id}/Roles")]
        public async Task<ActionResult<UserRolesApiDto<TRoleDto>>> GetUserRoles(TRoleDtoKey id, int page = 1, int pageSize = 10)
        {
            var userRoles = await _identityService.GetUserRolesAsync(id.ToString(), page, pageSize);
            var userRolesApiDto = _mapper.Map<UserRolesApiDto<TRoleDto>>(userRoles);

            return Ok(userRolesApiDto);
        }

        [HttpPost("Roles")]
        public async Task<IActionResult> PostUserRoles([FromBody] UserRoleApiDto<TUserDtoKey, TRoleDtoKey> role)
        {
            var userRolesDto = _mapper.Map<TUserRolesDto>(role);

            await _identityService.CreateUserRoleAsync(userRolesDto);

            return Ok();
        }

        [HttpDelete("Roles")]
        public async Task<IActionResult> DeleteUserRoles([FromBody] UserRoleApiDto<TUserDtoKey, TRoleDtoKey> role)
        {
            var userRolesDto = _mapper.Map<TUserRolesDto>(role);

            await _identityService.GetUserAsync(userRolesDto.UserId.ToString());
            await _identityService.GetRoleAsync(userRolesDto.RoleId.ToString());

            await _identityService.DeleteUserRoleAsync(userRolesDto);

            return Ok();
        }

        [HttpGet("{id}/Claims")]
        public async Task<ActionResult<UserClaimsApiDto<TUserDtoKey>>> GetUserClaims(TUserDtoKey id, int page = 1, int pageSize = 10)
        {
            var claims = await _identityService.GetUserClaimsAsync(id.ToString(), page, pageSize);

            var userClaimsApiDto = _mapper.Map<UserClaimsApiDto<TUserDtoKey>>(claims);

            return Ok(userClaimsApiDto);
        }

        [HttpPost("Claims")]
        public async Task<IActionResult> PostUserClaims([FromBody] UserClaimApiDto<TUserDtoKey> claim)
        {
            var userClaimDto = _mapper.Map<TUserClaimsDto>(claim);

            if (!userClaimDto.ClaimId.Equals(default))
            {
                return BadRequest(_errorResources.CannotSetId());
            }

            await _identityService.CreateUserClaimsAsync(userClaimDto);

            return Ok();
        }

        [HttpDelete("{id}/Claims")]
        public async Task<IActionResult> DeleteUserClaims([FromRoute] TUserDtoKey id, int claimId)
        {
            var userClaimsDto = new TUserClaimsDto
            {
                ClaimId = claimId,
                UserId = id
            };

            await _identityService.GetUserClaimAsync(id.ToString(), claimId);
            await _identityService.DeleteUserClaimsAsync(userClaimsDto);

            return Ok();
        }

        [HttpGet("{id}/Providers")]
        public async Task<ActionResult<UserProvidersApiDto<TUserDtoKey>>> GetUserProviders(TUserDtoKey id)
        {
            var userProvidersDto = await _identityService.GetUserProvidersAsync(id.ToString());
            var userProvidersApiDto = _mapper.Map<UserProvidersApiDto<TUserDtoKey>>(userProvidersDto);

            return Ok(userProvidersApiDto);
        }

        [HttpDelete("Providers")]
        public async Task<IActionResult> DeleteUserProviders([FromBody] UserProviderDeleteApiDto<TUserDtoKey> provider)
        {
            var providerDto = _mapper.Map<TUserProviderDto>(provider);

            await _identityService.GetUserProviderAsync(providerDto.UserId.ToString(), providerDto.ProviderKey);
            await _identityService.DeleteUserProvidersAsync(providerDto);

            return Ok();
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> PostChangePassword([FromBody] UserChangePasswordApiDto<TUserDtoKey> password)
        {
            var userChangePasswordDto = _mapper.Map<TUserChangePasswordDto>(password);

            await _identityService.UserChangePasswordAsync(userChangePasswordDto);

            return Ok();
        }

        [HttpGet("{id}/RoleClaims")]
        public async Task<ActionResult<RoleClaimsApiDto<TRoleDtoKey>>> GetRoleClaims(TUserDtoKey id, string claimSearchText, int page = 1, int pageSize = 10)
        {
            var roleClaimsDto = await _identityService.GetUserRoleClaimsAsync(id.ToString(), claimSearchText, page, pageSize);
            var roleClaimsApiDto = _mapper.Map<RoleClaimsApiDto<TRoleDtoKey>>(roleClaimsDto);

            return Ok(roleClaimsApiDto);
        }

        [HttpGet("TwoFactorAuthentication")]
        public async Task<IActionResult> TwoFactorAuthentication(string userName)
        {
            
            var user =string.IsNullOrWhiteSpace(userName) ?  await _userManager.GetUserAsync(User): await _userResolver.GetUserAsync(userName);
            if (user == null)
            {
                return NotFound(_localizer["UserNotFound", _userManager.GetUserId(User)]);
            }

            var model = new TwoFactorAuthenticationViewModel
            {
                HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null,
                Is2faEnabled = user.TwoFactorEnabled,
                RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user),
                IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user)
            };

            return new JsonResult(model);
        }
        [HttpGet("EnableAuthenticator")]
        public async Task<IActionResult> EnableAuthenticator(string userName)
        {
            var user = string.IsNullOrWhiteSpace(userName) ? await _userManager.GetUserAsync(User) : await _userResolver.GetUserAsync(userName);
            if (user == null)
            {
                return NotFound(_localizer["UserNotFound", _userManager.GetUserId(User)]);
            }

            var model = new EnableAuthenticatorViewModel();
            await LoadSharedKeyAndQrCodeUriAsync(user, model);

            return new JsonResult(model);
        }
        [HttpPost("EnableAuthenticator")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableAuthenticator(EnableAuthenticatorViewModel model)
        {
            var user = string.IsNullOrWhiteSpace(model.UserName) ? await _userManager.GetUserAsync(User) : await _userResolver.GetUserAsync(model.UserName);
            if (user == null)
            {
                return NotFound(_localizer["UserNotFound", _userManager.GetUserId(User)]);
            }

            if (!ModelState.IsValid)
            {
                await LoadSharedKeyAndQrCodeUriAsync(user, model);
                return new JsonResult(model);
            }

            var verificationCode = model.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2faTokenValid)
            {
                ModelState.AddModelError(_localizer["ErrorCode"], _localizer["InvalidVerificationCode"]);
                await LoadSharedKeyAndQrCodeUriAsync(user, model);
                return new JsonResult(model);
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);
            var userId = await _userManager.GetUserIdAsync(user);

            _logger.Information(_localizer["SuccessUserEnabled2FA"], userId);

            StatusMessage = _localizer["AuthenticatorVerified"];

            if (await _userManager.CountRecoveryCodesAsync(user) == 0)
            {
                var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
                
                var recoveryCodeModel = new ShowRecoveryCodesViewModel { RecoveryCodes = recoveryCodes.ToArray() };
                return new JsonResult(recoveryCodeModel);
            }

            return RedirectToAction(nameof(TwoFactorAuthentication));
        }
        
        private async Task LoadSharedKeyAndQrCodeUriAsync(TUser user, EnableAuthenticatorViewModel model)
        {
            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            model.SharedKey = FormatKey(unformattedKey);
            model.AuthenticatorUri = GenerateQrCodeUri(user.Email, unformattedKey);
        }

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            var currentPosition = 0;

            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }

            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                AuthenticatorUriFormat,
                _urlEncoder.Encode("Apotheek.Base.Identity.Api"),
                _urlEncoder.Encode(email),
                unformattedKey);
        }
    }
}