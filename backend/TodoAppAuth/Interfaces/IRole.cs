using Microsoft.AspNetCore.Mvc;
using TodoAppAuth.Dtos;
using TodoAppAuth.Models;

namespace TodoAppAuth.Interfaces
{
    public interface IRole
    {
        Task<IEnumerable<AppRole>> GetRoles();
        Task<AppRole> GetRole(int id);
        Task<ActionResult> EditRole(int id);
        Task<bool> DeleteRole(int id);

        Task<AppRole> CreateRole(CreateRoleDTO createRoleDTO);
    }
}
