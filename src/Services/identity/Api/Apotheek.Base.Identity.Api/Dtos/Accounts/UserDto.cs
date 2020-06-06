using System.Collections.Generic;

namespace Apotheek.Base.Identity.Api.Dtos.Accounts
{
    public class UserDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Initials { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsTwoFaEnabled { get; set; }
        public bool IsActive { get; set; }

        public IEnumerable<RoleDto> Roles { get; set; }
    }
}