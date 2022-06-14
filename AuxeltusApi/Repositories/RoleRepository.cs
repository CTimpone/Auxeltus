﻿using Auxeltus.AccessLayer.Sql;
using Auxeltus.Api.Extensions;
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
            _logger = logger;
        }

        public async Task<AuxeltusObjectResponse<Role>> CreateRoleAsync(Role newRole)
        {
            var response = new AuxeltusObjectResponse<Role>();

            if (newRole.Id.GetValueOrDefault() != 0)
            {
                response.AddError(ErrorType.Error, ErrorConstants.MODEL_VALIDATION_CODE,
                    "Id must be absent when creating a new Role", nameof(Role.Id));

                return response;
            }

            try
            {
                RoleEntity entity = await _roleCommand.CreateRoleAsync(MapToRoleEntity(newRole)).ConfigureAwait(false);

                response.Success = true;
                response.Content = MapRoleEntity(entity);
                return response;
            }
            catch (Exception ex)
            {
                if (ex.Contains(ErrorConstants.ROLE_TITLE_INDEX))
                {
                    _logger.LogError(ex, $"Title uniqueness exception thrown in {nameof(RoleRepository)}.{nameof(CreateRoleAsync)}");

                    response.AddError(ErrorType.Error, ErrorConstants.UNIQUENESS_CODE, 
                        "Title must not match the title of a pre-existing role", nameof(Role.Title));

                    return response;
                }
                
                throw;
            }
        }

        public async Task<AuxeltusObjectResponse> DeleteRoleAsync(int roleId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AuxeltusObjectResponse<Role>> RetrieveRoleAsync(int roleId)
        {
            var response = new AuxeltusObjectResponse<Role>();

            RoleEntity internalRole = await _roleQuery.RetrieveRoleAsync(roleId).ConfigureAwait(false);

            if (internalRole == null)
            {
                return null;
            }
            else
            {
                response.Success = true;
                response.Content = MapRoleEntity(internalRole);

                return response;
            }
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

        public async Task<AuxeltusObjectResponse<Role>> UpdateRoleAsync(int roleId, RolePatch role)
        {
            var response = new AuxeltusObjectResponse<Role>();

            try
            {
                RoleEntity internalRole = await _roleQuery.RetrieveRoleAsync(roleId).ConfigureAwait(false);

                if (role.TitleSpecified)
                {
                    internalRole.Title = role.Title;
                }

                if (role.TierSpecified)
                {
                    internalRole.Tier = role.Tier;
                }

                if (role.MaximumSalarySpecified)
                {
                    internalRole.MaximumSalary = role.MaximumSalary;
                }

                if (role.MinimumSalarySpecified)
                {
                    internalRole.MinimumSalary = role.MinimumSalary;
                }

                await _roleCommand.UpdateRoleAsync(roleId, internalRole).ConfigureAwait(false);

                response.Success = true;
                response.Content = MapRoleEntity(internalRole);

                return response;
            }
            catch (Exception ex)
            {
                if (ex.Contains(ErrorConstants.ROLE_TITLE_INDEX))
                {
                    _logger.LogError(ex, $"Title uniqueness exception thrown in {nameof(RoleRepository)}.{nameof(UpdateRoleAsync)}");

                    response.AddError(ErrorType.Error, ErrorConstants.UNIQUENESS_CODE,
                        "Title must not match the title of a pre-existing role", nameof(Role.Title));

                    return response;
                }

                throw;
            }
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
