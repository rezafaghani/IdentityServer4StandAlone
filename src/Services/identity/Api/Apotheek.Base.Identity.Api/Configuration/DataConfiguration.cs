using System.Collections.Generic;
using Apotheek.Base.Identity.Api.Configuration.Identity;

namespace Apotheek.Base.Identity.Api.Configuration
{
    public class DataConfiguration
    {
       public List<Role> Roles { get; set; }
       public List<User> Users { get; set; }
    }
}
