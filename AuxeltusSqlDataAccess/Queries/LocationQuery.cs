using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auxeltus.AccessLayer.Sql
{
    /// <summary>
    /// Class <c>LocationQuery</c> allows for the retrieval of <c>Location</c>s from the Auxeltus SQL data model.
    /// It utilizes asynchronous EF Core methodology to retrieve <c>Location</c>s from the SQL database.
    /// </summary>
    public class LocationQuery : ILocationQuery
    {
        private readonly ILogger _logger;
        private readonly AuxeltusSqlContext _context;

        public LocationQuery(ILogger logger, AuxeltusSqlContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Retrieves 
        /// It utilizes asynchronous EF Core methodology to retrieve <c>Location</c>s from the SQL database.
        /// Only returns a subset (default of 256) of the records, based on the <c>maxReturns</c> parameter.
        /// The <c>startIndex</c> parameter can be used to facilitate paging.
        /// </summary>
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