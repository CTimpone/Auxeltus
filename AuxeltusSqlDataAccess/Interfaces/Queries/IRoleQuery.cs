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
        /// Retrieves a subset of <c>Role</c> objects from the data store.
        /// By default will return the first 256 <c>Role</c>s.
        /// </summary>
        public Task<List<Role>> RetrieveRolesAsync(int? maxReturn, int? startIndex);
    }
}
