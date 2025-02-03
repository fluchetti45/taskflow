using TodoAppAuth.Dtos;
using TodoAppAuth.Models;

namespace TodoAppAuth.Interfaces
{
    public interface IAuth
    {
        Task<AppUser> SignupUser(RegisterUserDTO userDTO, string roleName = "User");
        Task<AppUser> LoginUser(LoginUserDTO userDTO);
    }
}
