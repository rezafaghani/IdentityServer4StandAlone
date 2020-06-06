using System.Threading.Tasks;
using Apotheek.Base.EntityFramework.Entities;
using Apotheek.Base.EntityFramework.Extensions.Common;
using IdentityServer4.EntityFramework.Entities;

namespace Apotheek.Base.EntityFramework.Identity.Repositories.Interfaces
{
	public interface IPersistedGrantAspNetIdentityRepository
    {
		Task<PagedList<PersistedGrantDataView>> GetPersistedGrantsByUsersAsync(string search, int page = 1, int pageSize = 10);
		Task<PagedList<PersistedGrant>> GetPersistedGrantsByUserAsync(string subjectId, int page = 1, int pageSize = 10);
	    Task<PersistedGrant> GetPersistedGrantAsync(string key);
	    Task<int> DeletePersistedGrantAsync(string key);
	    Task<int> DeletePersistedGrantsAsync(string userId);
        Task<bool> ExistsPersistedGrantsAsync(string subjectId);
        Task<bool> ExistsPersistedGrantAsync(string key);
        Task<int> SaveAllChangesAsync();
	}
}