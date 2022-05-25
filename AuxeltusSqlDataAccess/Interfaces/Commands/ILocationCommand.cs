using System.Threading.Tasks;

namespace Auxeltus.AccessLayer.Sql
{
    public interface ILocationCommand
    {
        public Task CreateLocationAsync(Location location);
        public Task UpdateLocationAsync(int locationId, Location location);
        public Task DeleteLocationAsync(int locationId);
    }
}
