using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auxeltus.AccessLayer.Sql
{
    /// <summary>
    /// The <c>IRoleQuery</c> interface provides an outline for the retrieval of <c>Role</c>s from the Auxeltus data model.
    /// </summary>
    public interface IRoleQuery
    {
        /// <summary>
        /// Retrieves a subset of <c>RoleEntity</c> objects from the data store.
        /// By default will return the first 256 <c>RoleEntity</c>s.
        /// </summary>
        public Task<List<RoleEntity>> RetrieveRolesAsync(int? maxReturn, int? startIndex);

        /// <summary>
        /// Retrieves a single <c>RoleEntity</c> object from the data store that matches the id input parameter.
        /// </summary>
        public Task<RoleEntity> RetrieveRoleAsync(int id);

    }
}
