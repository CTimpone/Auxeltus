using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Auxeltus.AccessLayer.Sql
{
    public class LocationCommand : ILocationCommand
    {
        private readonly ILogger _logger;
        private readonly AuxeltusSqlContext _context;

        public LocationCommand(ILogger logger, AuxeltusSqlContext context)
        {
            _logger = logger;
            _context = context;
        }

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