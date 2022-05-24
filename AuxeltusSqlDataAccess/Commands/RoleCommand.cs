using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Auxeltus.AccessLayer.Sql
{
    public class RoleCommand : IRoleCommand
    {
        private readonly ILogger _logger;
        private readonly AuxeltusSqlContext _context;

        public RoleCommand(ILogger logger, AuxeltusSqlContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task CreateRoleAsync(Role role)
        {
            try
            {
                await _context.Roles
                    .AddAsync(role)
                    .ConfigureAwait(false);

                await _context.SaveChangesAsync().ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in {nameof(RoleCommand)}.{nameof(CreateRoleAsync)}");
                throw;
            }
        }

        public async Task UpdateRoleAsync(int roleId, Role updatedRole)
        {
            try
            {
                Role foundRole = await _context.Roles
                    .FirstAsync(role => role.Id == roleId)
                    .ConfigureAwait(false);

                foundRole.Mutate(updatedRole);

                await _context.SaveChangesAsync().ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in {nameof(RoleCommand)}.{nameof(UpdateRoleAsync)}");
                throw;
            }
        }

        public async Task DeleteRoleAsync(int roleId)
        {
            try
            {
                Role foundRole = await _context.Roles
                    .FirstAsync(role => role.Id == roleId)
                    .ConfigureAwait(false);

                _context.Remove(foundRole);
                await _context.SaveChangesAsync().ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in {nameof(RoleCommand)}.{nameof(DeleteRoleAsync)}");
                throw;
            }
        }
    }
}