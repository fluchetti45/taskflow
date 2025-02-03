using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoAppAuth.Context;
using TodoAppAuth.Dtos;
using TodoAppAuth.Interfaces;
using TodoAppAuth.Models;

namespace TodoAppAuth.Services
{
    public class AuthService : IAuth
    {
        private readonly TodoContext _todoContext;
        private readonly PasswordHasher<AppUser> _passwordHasher;
        private readonly IConfiguration _configuration;
        public AuthService(TodoContext todoContext, PasswordHasher<AppUser> passwordHasher, IConfiguration configuration)
        {
            _todoContext = todoContext;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public string GenerateToken(int userId, string email, string roleName)
        {
            var claims = new List<Claim>
            {
                // No uso los claims por defecto porque desde el frontend no les puedo leer los nombres.
                new Claim("id", userId.ToString()),
                new Claim("email", email),
                new Claim("role", roleName)

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<AppUser> LoginUser(LoginUserDTO userDTO)
        {
            // Tengo que buscar un usuario con el mail ingresado y verificar que tiene la misma password
            AppUser user = await this._todoContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == userDTO.Email);
            if (user == null)
            {
                return null;
            }
            // Validar que las contrasenias coinciden..
            var result = this._passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userDTO.Password);
            if (result == PasswordVerificationResult.Success)
            {
                return user;
            }
            return null;

        }

        public async Task<AppUser> SignupUser(RegisterUserDTO userDTO, string roleName = "User")
        {
            // se valida que las contrasenias coincidan
            if (userDTO.Password1 != userDTO.Password2)
            {
                throw new Exception("Las contraseñas no coinciden");
            }
            // Se valida que no este tomado el email.
            AppUser user = this._todoContext.Users.FirstOrDefault(u => u.Email == userDTO.Email);
            if (user != null)
            {
                throw new Exception("El mail ya esta en uso");
            }
            // Hasheo la contrasenia
            var hashedPassword = this._passwordHasher.HashPassword(null, userDTO.Password1);
            // Creo el usuario
            AppUser newUser = new AppUser
            {
                Email = userDTO.Email,
                PasswordHash = hashedPassword,
                IsActive = true,
                CreatedAt = DateOnly.FromDateTime(DateTime.Now),
            };
            // Asigno rol por defecto.
            AppRole employeeRole = await this._todoContext.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
            if (employeeRole == null)
            {
                throw new Exception("El rol por defecto no existe. Contacta a soporte.");
            }
            newUser.RoleId = employeeRole.Id;
            this._todoContext.Users.Add(newUser);
            await this._todoContext.SaveChangesAsync();
            return newUser;
        }
    }
}
