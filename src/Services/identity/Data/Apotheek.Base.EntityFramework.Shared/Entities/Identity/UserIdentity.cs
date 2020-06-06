using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Apotheek.Base.EntityFramework.Shared.Entities.Identity
{
    public class UserIdentity : IdentityUser<long>
    {
        #region | Properties |

        public int PinCode { get; set; }
        public int CustomerId { get; set; }
        public int PharmacyId { get; set; }

        [Column(TypeName = "jsonb")]
        public string Cluster { get; set; }

        #endregion | Properties |
    }
}