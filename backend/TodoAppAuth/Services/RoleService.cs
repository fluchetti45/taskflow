using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAppAuth.Context;
using TodoAppAuth.Dtos;
using TodoAppAuth.Interfaces;
using TodoAppAuth.Models;

namespace TodoAppAuth.Services
{
    public class RoleService : IRole
    {
        private readonly TodoContext _todoContext;
        public RoleService(TodoContext context)
        {
            _todoContext = context;
        }
        public async Task<IEnumerable<AppRole>> GetRoles()
        {
            IEnumerable<AppRole> roles = await this._todoContext.Roles.ToListAsync();
            return roles;
        }

        public async Task<AppRole> GetRole(int id)
        {
            AppRole role = await this._todoContext.Roles.FirstOrDefaultAsync(r => r.Id == id);
            return role;
        }

        public async Task<AppRole> CreateRole(CreateRoleDTO createRoleDTO)
        {
            // Crear el rol. Verificar que se puede crear..
            AppRole newRole = new AppRole
            {
                RoleName = createRoleDTO.roleName,
                IsActive = true,
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            };
            this._todoContext.Roles.Add(newRole);
            await this._todoContext.SaveChangesAsync();
            return newRole;
        }

        public async Task<bool> DeleteRole(int id)
        {
            AppRole role = await this._todoContext.Roles.FirstOrDefaultAsync(r => r.Id == id);
            if(role != null)
            {
                role.IsActive = false;
                int changed = await this._todoContext.SaveChangesAsync();
                return changed > 0;
            }
            return false;
        }

        public Task<ActionResult> EditRole(int id)
        {
            throw new NotImplementedException();
        }

        
    }
}
