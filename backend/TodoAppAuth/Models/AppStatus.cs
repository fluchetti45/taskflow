using System.Text.Json.Serialization;

namespace TodoAppAuth.Models
{
    public class AppStatus
    {
        public int Id { get; set; }
        public string StatusName { get; set; }

        public bool IsActive { get; set; }

        public DateOnly CreatedAt { get; set; }

        public DateOnly UpdatedAt { get; set; }

        [JsonIgnore]
        public ICollection<Tarea> Tareas { get; set; }
    }
}
