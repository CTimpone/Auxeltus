using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Auxeltus.AccessLayer.Sql
{
    /// <summary>
    /// Class <c>RoleCommand</c> allows for the modification of the Auxeltus SQL data model, specifically with regards to <c>Role</c>.
    /// It utilizes asynchronous EF Core methodology to add, update, and delete items from the SQL database.
    /// </summary>
    public class RoleCommand : IRoleCommand
    {
        private readonly ILogger _logger;
        private readonly AuxeltusSqlContext _context;

        public RoleCommand(ILogger<RoleCommand> logger, AuxeltusSqlContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Adds a new <c>Role</c> to the data structure.
        /// Input object should not include an Id (as it is auto-generated as the primary key).
        /// </summary>
        public async Task CreateRoleAsync(RoleEntity role)
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
                string message = $"Exception thrown in {nameof(RoleCommand)}.{nameof(CreateRoleAsync)}";
                _logger.LogError(ex, message);
                throw new AuxeltusSqlException(message, ex);
            }
        }

        /// <summary>
        /// Updates an existing <c>Role</c> on the data structure.
        /// </summary>
        public async Task UpdateRoleAsync(int roleId, RoleEntity updatedRole)
        {
            try
            {
                RoleEntity foundRole = await _context.Roles
                    .FirstAsync(role => role.Id == roleId)
                    .ConfigureAwait(false);

                foundRole.Mutate(updatedRole);

                await _context.SaveChangesAsync().ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                string message = $"Exception thrown in {nameof(RoleCommand)}.{nameof(UpdateRoleAsync)}";
                _logger.LogError(ex, message);
                throw new AuxeltusSqlException(message, ex);
            }
        }

        /// <summary>
        /// Deletes an existing <c>Role</c> on the data structure.
        /// </summary>
        public async Task DeleteRoleAsync(int roleId)
        {
            try
            {
                RoleEntity foundRole = await _context.Roles
                    .FirstAsync(role => role.Id == roleId)
                    .ConfigureAwait(false);

                _context.Remove(foundRole);
                await _context.SaveChangesAsync().ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                string message = $"Exception thrown in {nameof(RoleCommand)}.{nameof(DeleteRoleAsync)}";
                _logger.LogError(ex, message);
                throw new AuxeltusSqlException(message, ex);
            }
        }
    }
}