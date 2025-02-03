using System.Text.Json.Serialization;

namespace TodoAppAuth.Models
{
    public class Tarea
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int StatusId { get; set; }

        [JsonIgnore]
        public AppStatus Status { get; set; }

        public int UserId { get; set; }

        [JsonIgnore]
        public AppUser User { get; set; }
        
    }
}
