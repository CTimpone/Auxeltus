using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auxeltus.AccessLayer.Sql
{
    public class LocationQuery : ILocationQuery
    {
        private readonly ILogger _logger;
        private readonly AuxeltusSqlContext _context;

        public LocationQuery(ILogger logger, AuxeltusSqlContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<List<Location>> RetrieveLocationsAsync(int? maxReturns, int? startIndex)
        {
            maxReturns ??= 256;
            startIndex ??= 0;

            try
            {
                List<Location> locations = await _context.Locations
                    .AsNoTracking()
                    .Where(location => location.Id > startIndex)
                    .OrderBy(location => location.Id)
                    .Take(maxReturns.Value)
                    .ToListAsync()
                    .ConfigureAwait(false);

                return locations;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in {nameof(LocationQuery)}.{nameof(RetrieveLocationsAsync)}");
                throw;
            }
        }
    }
}