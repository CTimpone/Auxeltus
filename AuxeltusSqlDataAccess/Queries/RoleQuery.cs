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

        public RoleQuery(ILogger logger, AuxeltusSqlContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// It utilizes asynchronous EF Core methodology to retrieve <c>Role</c>s from the SQL database.
        /// Only returns a subset (default of 256) of the records, based on the <c>maxReturns</c> parameter.
        /// The <c>startIndex</c> parameter can be used to facilitate paging.
        /// </summary>
        public async Task<List<Role>> RetrieveRolesAsync(int? maxReturns, int? startIndex)
        {
            maxReturns ??= 256;
            startIndex ??= 0;

            try
            {
                List<Role> roles = await _context.Roles
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