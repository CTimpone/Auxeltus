using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auxeltus.AccessLayer.Sql
{
    /// <summary>
    /// Class <c>RoleQuery</c> allows for the retrieval of <c>Role</c>s from the Auxeltus SQL data model.
    /// It utilizes asynchronous EF Core methodology to retrieve <c>Role</c>s from the SQL database.
    /// </summary>
    public class RoleQuery : IRoleQuery
    {
        private readonly ILogger _logger;
        private readonly AuxeltusSqlContext _context;

        public RoleQuery(ILogger<RoleQuery> logger, AuxeltusSqlContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// It utilizes asynchronous EF Core methodology to retrieve a single <c>RoleEntity</c> from the SQL database.
        /// The returned entity will have an Id value that matches the input parameter.
        /// </summary>
        public async Task<RoleEntity> RetrieveRoleAsync(int id)
        {
            try
            {
                RoleEntity role = await _context.Roles
                    .AsNoTracking()
                    .FirstOrDefaultAsync(role => role.Id == id)
                    .ConfigureAwait(false);

                return role;
            }
            catch (Exception ex)
            {
                string message = $"Exception thrown in {nameof(RoleQuery)}.{nameof(RetrieveRoleAsync)}";
                _logger.LogError(ex, message);
                throw new AuxeltusSqlException(message, ex);
            }
        }

        /// <summary>
        /// It utilizes asynchronous EF Core methodology to retrieve <c>RoleEntity</c>s from the SQL database.
        /// Only returns a subset (default of 256) of the records, based on the <c>maxReturns</c> parameter.
        /// The <c>startIndex</c> parameter can be used to facilitate paging.
        /// </summary>
        public async Task<List<RoleEntity>> RetrieveRolesAsync(int? maxReturns, int? startIndex)
        {
            maxReturns ??= 256;
            startIndex ??= 0;

            try
            {
                List<RoleEntity> roles = await _context.Roles
                    .AsNoTracking()
                    .Where(role => role.Id >= startIndex)
                    .OrderBy(job => job.Id)
                    .Take(maxReturns.Value)
                    .ToListAsync()
                    .ConfigureAwait(false);

                return roles;
            }
            catch (Exception ex)
            {
                string message = $"Exception thrown in {nameof(RoleQuery)}.{nameof(RetrieveRolesAsync)}";
                _logger.LogError(ex, message);
                throw new AuxeltusSqlException(message, ex);
            }
        }

    }
}