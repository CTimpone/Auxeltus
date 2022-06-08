using Auxeltus.AccessLayer.Sql;
using Auxeltus.Api.Interfaces;
using Auxeltus.Api.Models;
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
            var response = new AuxeltusObjectResponse<Role>();

            if (newRole.Id.GetValueOrDefault() != 0)
            {
                response.AddError(new Error
                {
                    Code = 1,
                    Field = "Id",
                    Message = "Id must be absent when creating a new Role",
                    Type = ErrorType.Error
                });

                return response;
            }

            try
            {
                await _roleCommand.CreateRoleAsync(MapToRoleEntity(newRole)).ConfigureAwait(false);

                response.Success = true;

                return response;   
            }
            catch (Exception ex)
            {
                if (ex.InnerException?.InnerException?.Message?.Contains("IX_Roles_Title") == true)
                {
                    response.AddError(new Error
                    {
                        Code = 3,
                        Field = "Title",
                        Message = "Title must not match a pre-existing role",
                        Type = ErrorType.Error
                    });

                    return response;
                }
                
                throw;
            }
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
            return internalRoles.Select(r => MapRoleEntity(r)).ToList();
        }

        private Role MapRoleEntity(RoleEntity internalRole)
        {
            return new Role
            {
                Id = internalRole.Id,
                Title = internalRole.Title,
                Tier = internalRole.Tier.GetValueOrDefault(),
                MaximumSalary = internalRole.MaximumSalary.GetValueOrDefault(),
                MinimumSalary = internalRole.MinimumSalary.GetValueOrDefault()
            };
        }

        private RoleEntity MapToRoleEntity(Role role)
        {
            return new RoleEntity
            {
                Id = role.Id.GetValueOrDefault(),
                Title = role.Title,
                Tier = role.Tier.GetValueOrDefault(),
                MaximumSalary= role.MaximumSalary.GetValueOrDefault(),
                MinimumSalary = role.MinimumSalary.GetValueOrDefault()
            };
        }
    }
}
