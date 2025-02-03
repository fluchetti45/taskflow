namespace TodoAppAuth.Dtos
{
    public class CreateTaskDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public int statusId { get; set; }
    }
}
