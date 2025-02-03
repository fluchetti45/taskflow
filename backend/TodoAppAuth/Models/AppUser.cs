namespace TodoAppAuth.Models
{
    public class AppUser
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public bool IsActive { get; set; }

        public DateOnly CreatedAt { get; set; }

        public DateOnly UpdatedAt { get; set; }

        public int RoleId { get; set; }


        public AppRole Role { get; set; }


        public ICollection<Tarea> Tareas { get; set; }
    }
}
