using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auxeltus.AccessLayer.Sql
{
    public class RoleQuery : IRoleQuery
    {
        private readonly ILogger _logger;
        private readonly AuxeltusSqlContext _context;

        public RoleQuery(ILogger logger, AuxeltusSqlContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<List<Role>> RetrieveRolesAsync(int? maxReturns, int? startIndex)
        {
            maxReturns ??= 256;
            startIndex ??= 0;

            try
            {
                List<Role> roles = await _context.Roles
                    .AsNoTracking()
                    .Where(role => role.Id > startIndex)
                    .OrderBy(job => job.Id)
                    .Take(maxReturns.Value)
                    .ToListAsync()
                    .ConfigureAwait(false);

                return roles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in {nameof(RoleQuery)}.{nameof(RetrieveRolesAsync)}");
                throw;
            }
        }

    }
}