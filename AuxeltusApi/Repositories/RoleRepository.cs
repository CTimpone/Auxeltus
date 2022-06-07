using Auxeltus.AccessLayer.Sql;
using Auxeltus.Api.Interfaces;
using Auxeltus.Api.Models;
using Auxeltus.Api.Models.Exposed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<AuxeltusObjectResponse<Role>> CreateRoleAsync(Role newRole)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AuxeltusObjectResponse> DeleteRoleAsync(int roleId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AuxeltusObjectResponse<List<Role>>> RetrieveRolesAsync(int? maxReturn, int? startIndex)
        {
            var response = new AuxeltusObjectResponse<List<Role>>();

            List<RoleEntity> internalRoles = await _roleQuery.RetrieveRolesAsync(maxReturn, startIndex).ConfigureAwait(false);

            if (internalRoles == null || internalRoles.Count == 0)
            {
                return null;
            }
            else
            {
                response.Success = true;
                response.Content = MapRoleEntities(internalRoles);

                return response;
            }
        }

        public async Task<AuxeltusObjectResponse<Role>> UpdateRoleAsync(int roleId, Role role)
        {
            throw new System.NotImplementedException();
        }

        private List<Role> MapRoleEntities(List<RoleEntity> internalRoles)
        {
            return internalRoles.Select(r => new Role
            {
                Id = r.Id,
                Title = r.Title,
                Tier = r.Tier.GetValueOrDefault(),
                MaximumSalary = r.MaximumSalary.GetValueOrDefault(),
                MinimumSalary = r.MinimumSalary.GetValueOrDefault()
            }).ToList();
        }

    }
}
