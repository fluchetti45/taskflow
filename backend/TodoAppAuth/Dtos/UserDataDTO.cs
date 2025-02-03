namespace TodoAppAuth.Dtos
{
    public class UserDataDTO
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public DateOnly CreatedAt { get; set; }

        public string RoleName { get; set; }

    }
}
