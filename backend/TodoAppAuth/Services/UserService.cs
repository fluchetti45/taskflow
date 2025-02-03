using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TodoAppAuth.Context;
using TodoAppAuth.Dtos;
using TodoAppAuth.Interfaces;
using TodoAppAuth.Models;

namespace TodoAppAuth.Services
{
    public class UserService : IUser
    {
       private readonly TodoContext _todoContext;
        public UserService(TodoContext context)
        {
            _todoContext = context;
        }
        
        // OBTENER USUARIO
        public async Task<AppUser> GetUser(int id)
        {
            AppUser user = await this._todoContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }
        // OBTENER TODOS LOS USUARIOS
        public async Task<IEnumerable<AppUser>> GetUsers()
        {
            var users = await this._todoContext.Users
                .Select(u => new AppUser
                {
                    Id = u.Id,
                    CreatedAt = u.CreatedAt,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    Role = new AppRole
                    {
                        RoleName = u.Role.RoleName
                    },
                })
                .ToListAsync();

            return users;
        }

        // OBTNER DTO DE USUARIO
        public async Task<UserDataDTO> GetUserData(int id)
        {
            AppUser user = await this._todoContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
            UserDataDTO userData = new UserDataDTO
            {
                Id = user.Id,
                CreatedAt = user.CreatedAt,
                Email = user.Email,
                IsActive = user.IsActive,
                RoleName = user.Role.RoleName,
            };
            return userData;
        }
        // OBTENER TODOS LOS DTO DE USUARIOS
        public async Task<IEnumerable<UserDataDTO>> GetAllUsersData()
        {
            List<AppUser> users = await this._todoContext.Users
                .Select(u => new AppUser
                {
                    Id = u.Id,
                    CreatedAt = u.CreatedAt,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    Role = new AppRole
                    {
                        RoleName = u.Role.RoleName
                    },
                })
                .ToListAsync();

            // Mapeo de AppUser a UserDataDTO
            IEnumerable<UserDataDTO> usersData = users.Select(u => new UserDataDTO
            {
                Id = u.Id,
                CreatedAt = u.CreatedAt,
                Email = u.Email,
                IsActive = u.IsActive,
                RoleName = u.Role.RoleName
            });
            return usersData;
        }

        // CAMBIO EL ROL DEL USUARIO
        public async Task<UserDataDTO> UpdateUserRole(int userId, string roleName)
        {
            AppUser user = await this._todoContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return null;
            }
            AppRole role = await this._todoContext.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
            if (role == null)
            {
                return null;
            }
            user.Role = role;
            user.RoleId = role.Id;
            await this._todoContext.SaveChangesAsync();
            UserDataDTO userData = new UserDataDTO
            {
                Id = user.Id,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                RoleName = user.Role.RoleName,
                IsActive = user.IsActive,
            };
            return userData;

        }

        // ACTIVO EL USUARIO
        public async Task<bool> ReactivateUser(AppUser user)
        {
            if (user == null)
            {
                return false;
            }
            user.IsActive = true;
            await this._todoContext.SaveChangesAsync();
            return true;
        }
        // DESACTIVO EL USUARIO
        public async Task<bool> DeactivateUser(AppUser user)
        {
            if (user == null)
            {
                return false;
            }
            user.IsActive = false;
            await this._todoContext.SaveChangesAsync();
            return true;
        }
        // BORRO EL USUARIO
        public async Task<bool> DeleteUser(int id)
        {
            AppUser user = await _todoContext.Users.FindAsync(id); // Uso de FindAsync
            if (user != null)
            {
                _todoContext.Users.Remove(user);
                await _todoContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}
