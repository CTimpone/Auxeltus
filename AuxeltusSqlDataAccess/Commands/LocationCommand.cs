using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Auxeltus.AccessLayer.Sql
{
    /// <summary>
    /// Class <c>LocationCommand</c> allows for the modification of the Auxeltus SQL data model, specifically with regards to <c>Location</c>.
    /// It utilizes asynchronous EF Core methodology to add, update, and delete items from the SQL database.
    /// </summary>
    public class LocationCommand : ILocationCommand
    {
        private readonly ILogger _logger;
        private readonly AuxeltusSqlContext _context;

        public LocationCommand(ILogger logger, AuxeltusSqlContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Adds a new <c>Location</c> to the data structure.
        /// Input object should not include an Id (as it is auto-generated as the primary key).
        /// </summary>
        public async Task CreateLocationAsync(Location location)
        {
            try
            {
                await _context.Locations
                    .AddAsync(location)
                    .ConfigureAwait(false);

                await _context.SaveChangesAsync().ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in {nameof(LocationCommand)}.{nameof(CreateLocationAsync)}");
                throw;
            }
        }

        /// <summary>
        /// Updates an existing <c>Location</c> on the data structure.
        /// </summary>
        public async Task UpdateLocationAsync(int locationId, Location updatedLocation)
        {
            try
            {
                Location foundLocation = await _context.Locations
                    .FirstAsync(loc => loc.Id == locationId)
                    .ConfigureAwait(false);

                foundLocation.Mutate(updatedLocation);

                await _context.SaveChangesAsync().ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in {nameof(LocationCommand)}.{nameof(UpdateLocationAsync)}");
                throw;
            }
        }

        /// <summary>
        /// Deletes an existing <c>Location</c> on the data structure.
        /// </summary>
        public async Task DeleteLocationAsync(int locationId)
        {
            try
            {
                Location foundLocation = await _context.Locations
                    .FirstAsync(loc => loc.Id == locationId)
                    .ConfigureAwait(false);

                _context.Remove(foundLocation);
                await _context.SaveChangesAsync().ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in {nameof(LocationCommand)}.{nameof(DeleteLocationAsync)}");
                throw;
            }
        }
    }
}