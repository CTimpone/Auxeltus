using System.Threading.Tasks;

namespace Auxeltus.AccessLayer.Sql
{
    /// <summary>
    /// The <c>IRoleCommand</c> interface provides an outline for the modification of the Auxeltus data model, specifically with regards to <c>Role</c> model.
    /// </summary>
    public interface IRoleCommand
    {
        /// <summary>
        /// Adds a new <c>Role</c> to the data structure.
        /// Input object should not include an Id (as it is auto-generated as the primary key).
        /// </summary>
        public Task CreateRoleAsync(Role role);

        /// <summary>
        /// Updates an existing <c>Role</c> on the data structure.
        /// </summary>
        public Task UpdateRoleAsync(int roleId, Role role);

        /// <summary>
        /// Deletes an existing <c>Role</c> on the data structure.
        /// </summary>
        public Task DeleteRoleAsync(int roleId);
    }
}
