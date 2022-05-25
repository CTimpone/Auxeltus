using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auxeltus.AccessLayer.Sql
{
    /// <summary>
    /// The <c>ILocationQuery</c> interface provides an outline for the retrieval of <c>Location</c>s from the Auxeltus data model.
    /// </summary>
    public interface ILocationQuery
    {
        /// <summary>
        /// Retrieves a subset of <c>Location</c> objects from the data store.
        /// By default will return the first 256 <c>Location</c>s.
        /// </summary>
        public Task<List<Location>> RetrieveLocationsAsync(int? maxReturn, int? startIndex);
    }
}
