using Microsoft.AspNetCore.Identity;
using TodoAppAuth.Models;

namespace TodoAppAuth.Context
{
    public static class DbInitializer
    {
        public static void Initializer(TodoContext context)
        {
            context.Database.EnsureCreated(); // Aplica migraciones antes de insertar datos

            // Insertar Roles si no existen
            if (!context.Roles.Any())
            {
                var roles = new AppRole[]
                {
                    new AppRole {
                        RoleName = "Admin",
                        CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                        UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                        IsActive = true
                    },
                    new AppRole {
                        RoleName = "User",
                        CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                        UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                        IsActive = true
                    }
                };

                context.Roles.AddRange(roles);
                context.SaveChanges();
            }

            // Insertar Statuses si no existen
            if (!context.Statuses.Any())
            {
                var statuses = new AppStatus[]
                {
                    new AppStatus {

                        StatusName = "Pendiente",
                        CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                        UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                        IsActive = true
                    },
                    new AppStatus {

                        StatusName = "En proceso",
                        CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                        UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                        IsActive = true
                    },
                    new AppStatus {

                        StatusName = "Completada",
                        CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                        UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                        IsActive = true
                    },
                    new AppStatus {

                        StatusName = "Cancelada",
                        CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                        UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                        IsActive = true
                    }
                };

                context.Statuses.AddRange(statuses);
                context.SaveChanges();
            }

            // Insertar Usuarios si no existen
            if (!context.Users.Any())
            {
                var passwordHasher = new PasswordHasher<AppUser>();

                var users = new AppUser[]
                {
                    new AppUser{
                        Email = "admin@admin.com",
                        RoleId = 1,
                        PasswordHash = passwordHasher.HashPassword(null, "contrasenia"),
                        CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                        UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                        IsActive = true
                    },
                    new AppUser{
                        Email = "user@user.com",
                        RoleId = 2,
                        PasswordHash = passwordHasher.HashPassword(null, "contrasenia"),
                        CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                        UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                        IsActive = true
                    }
                };

                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }
    }
}
