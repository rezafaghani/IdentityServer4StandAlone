using System;
using System.ComponentModel.DataAnnotations;
using Apotheek.Base.BusinessLogic.Identity.Dtos.Identity.Base;
using Apotheek.Base.BusinessLogic.Identity.Dtos.Identity.Interfaces;

namespace Apotheek.Base.BusinessLogic.Identity.Dtos.Identity
{
    public class UserDto<TKey> : BaseUserDto<TKey>, IUserDto
    {        
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_@\-\.\+]+$")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool LockoutEnabled { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }
        public int PinCode { get; set; }
        public int CustomerId { get; set; }
        public int PharmacyId { get; set; }

        public string Cluster { get; set; }
    }
}
