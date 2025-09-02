namespace Dentlike.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }   // Primary key
        public required string Name { get; set; }
        public string? Email { get; set; }

        public int age { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
