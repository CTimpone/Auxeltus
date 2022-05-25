using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auxeltus.AccessLayer.Sql
{
    public interface ILocationQuery
    {
        public Task<List<Location>> RetrieveLocationsAsync(int? maxReturn, int? startIndex);
    }
}
