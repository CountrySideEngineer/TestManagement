namespace TestManagement.API.Models
{
    public class Environment
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Os { get; set; }

        public string? RunTime { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
