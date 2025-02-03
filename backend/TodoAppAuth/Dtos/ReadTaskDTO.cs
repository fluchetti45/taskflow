namespace TodoAppAuth.Dtos
{
    public class ReadTaskDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string StatusName { get; set; }

        public int StatusId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
