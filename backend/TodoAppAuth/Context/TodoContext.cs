using Microsoft.EntityFrameworkCore;
using TodoAppAuth.Models;

namespace TodoAppAuth.Context
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }

        // Db sets
        public DbSet<AppUser> Users { get; set; }
        public DbSet<AppRole> Roles { get; set; }
        public DbSet<AppStatus> Statuses { get; set; }
        public DbSet<Tarea> Tasks { get; set; }
        // Config

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Obtengo la entidad AppUser
            var users = modelBuilder.Entity<AppUser>();
            // Defino el nombre de la tabla
            users.ToTable("Users");
            // Defino la pk
            users.HasKey(u => u.Id);
            // Defino email como requerido y unico.
            users.Property(u => u.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(50);
            users.HasIndex(u => u.Email)
                .IsUnique();
            // Defino valor por defecto de estado del usuario
            users.Property(u => u.IsActive)
                .HasDefaultValue(true);
            //
            users.HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId);
            //
            users.HasMany(u => u.Tareas)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId);

            // AppRole
            var roles = modelBuilder.Entity<AppRole>();
            //
            roles.ToTable("Roles");
            //
            roles.HasKey(r => r.Id);
            //
            roles.Property(r => r.RoleName)
                .HasMaxLength(20)
                .IsRequired();
            roles.HasIndex(r => r.RoleName)
                .IsUnique();

            // AppStatus
            var appStatus = modelBuilder.Entity<AppStatus>();
            //
            appStatus.ToTable("Statuses");
            //
            appStatus.HasKey(a => a.Id);
            //
            appStatus.Property(a => a.StatusName)
                .HasMaxLength(20)
                .IsRequired();
            appStatus.HasIndex(a => a.StatusName)
                .IsUnique();

            // Tareas
            var tareas = modelBuilder.Entity<Tarea>();
            // Table name
            tareas.ToTable("Tasks");
            // PK
            tareas.HasKey(t => t.Id);
            //
            tareas.Property(t => t.Title)
                .HasMaxLength(50)
                .IsRequired();
            tareas.Property(t => t.Description)
                .HasMaxLength(255);
            // Relacion con Users
            tareas.HasOne(t => t.User)
                .WithMany(u => u.Tareas)
                .HasForeignKey(t => t.UserId);
            // Relacion con Status
            tareas.HasOne(t => t.Status)
                .WithMany(s => s.Tareas)
                .HasForeignKey(t => t.StatusId);


            // Roles
            modelBuilder.Entity<AppRole>().HasData([
                new AppRole {
                    Id = 1,
                    RoleName = "Admin",
                    CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                    UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                    IsActive = true,
                },
                new AppRole {
                    Id = 2,
                    RoleName = "User",
                    CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                    UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                    IsActive = true,
                }
                ]);
            // Statuses
            modelBuilder.Entity<AppStatus>().HasData([
                new AppStatus {
                    Id = 1,
                    StatusName = "Pendiente",
                    CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                    UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                    IsActive = true
                },
                new AppStatus {
                    Id = 2,
                    StatusName = "En proceso",
                    CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                    UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                    IsActive = true
                },
                new AppStatus {
                    Id = 3,
                    StatusName = "Completada",
                    CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                    UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                    IsActive = true
                },
                new AppStatus {
                    Id = 4,
                    StatusName = "Cancelada",
                    CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                    UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                    IsActive = true
                },
                ]);
        }
    }
}
