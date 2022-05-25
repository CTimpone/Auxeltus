using System.Threading.Tasks;

namespace Auxeltus.AccessLayer.Sql
{
    /// <summary>
    /// The <c>ILocationCommand</c> interface provides an outline for the modification of the Auxeltus data model, specifically with regards to <c>Location</c> model.
    /// </summary>
    public interface ILocationCommand
    {
        /// <summary>
        /// Adds a new <c>Location</c> to the data structure.
        /// Input object should not include an Id (as it is auto-generated as the primary key).
        /// </summary>
        public Task CreateLocationAsync(Location location);

        /// <summary>
        /// Updates an existing <c>Location</c> on the data structure.
        /// </summary>
        public Task UpdateLocationAsync(int locationId, Location location);

        /// <summary>
        /// Deletes an existing <c>Location</c> on the data structure.
        /// </summary>
        public Task DeleteLocationAsync(int locationId);
    }
}
