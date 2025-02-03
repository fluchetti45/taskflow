using Microsoft.AspNetCore.Mvc;
using TodoAppAuth.Dtos;
using TodoAppAuth.Models;

namespace TodoAppAuth.Interfaces
{
    public interface IUser
    {
        Task<AppUser> GetUser(int id);
        Task<IEnumerable<AppUser>> GetUsers();

        Task<UserDataDTO> GetUserData(int id);

        Task<IEnumerable<UserDataDTO>> GetAllUsersData();

        Task<UserDataDTO> UpdateUserRole(int userId, string role);
        Task<bool> ReactivateUser(AppUser user);

        Task<bool> DeactivateUser(AppUser user);
        
        Task<bool> DeleteUser(int id);
    }
}
