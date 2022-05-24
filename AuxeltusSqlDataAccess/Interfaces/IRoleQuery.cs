using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auxeltus.AccessLayer.Sql
{
    public interface IRoleQuery
    {
        public Task<List<Role>> RetrieveRolesAsync(int? maxReturn, int? startIndex);
    }
}
