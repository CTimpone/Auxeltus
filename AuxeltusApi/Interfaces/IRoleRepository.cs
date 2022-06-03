using Auxeltus.Api.Models;
using Auxeltus.Api.Models.Exposed;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auxeltus.Api.Interfaces
{
    public interface IRoleRepository
    {
        public Task<AuxeltusObjectResponse<List<Role>>> RetrieveRolesAsync(int? maxReturn, int? startIndex);
        public Task<AuxeltusObjectResponse<Role>> CreateRoleAsync(Role newRole);
        public Task<AuxeltusObjectResponse<Role>> UpdateRoleAsync(int roleId, Role role);
        public Task<AuxeltusObjectResponse> DeleteRoleAsync(int roleId);


    }
}
