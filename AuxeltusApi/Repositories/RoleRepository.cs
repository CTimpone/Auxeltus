using Auxeltus.AccessLayer.Sql;
using Auxeltus.Api.Interfaces;
using Auxeltus.Api.Models;
using Auxeltus.Api.Models.Exposed;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auxeltus.Api
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IRoleCommand _roleCommand;
        private readonly IRoleQuery _roleQuery;

        private readonly ILogger _logger;

        public RoleRepository(IRoleCommand roleCommand, IRoleQuery roleQuery, ILogger logger)
        {
            _roleCommand = roleCommand;
            _roleQuery = roleQuery;
        }

        public Task<AuxeltusObjectResponse<Role>> CreateRoleAsync(Role newRole)
        {
            throw new System.NotImplementedException();
        }

        public Task<AuxeltusObjectResponse> DeleteRoleAsync(int roleId)
        {
            throw new System.NotImplementedException();
        }

        public Task<AuxeltusObjectResponse<List<Role>>> RetrieveRolesAsync(int? maxReturn, int? startIndex)
        {
            throw new System.NotImplementedException();
        }

        public Task<AuxeltusObjectResponse<Role>> UpdateRoleAsync(int roleId, Role role)
        {
            throw new System.NotImplementedException();
        }
    }
}
