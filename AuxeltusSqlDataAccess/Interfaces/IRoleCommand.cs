using System.Threading.Tasks;

namespace Auxeltus.AccessLayer.Sql
{
    public interface IRoleCommand
    {
        public Task CreateRoleAsync(Role role);
        public Task UpdateRoleAsync(int roleId, Role role);
        public Task DeleteRoleAsync(int roleId);
    }
}
