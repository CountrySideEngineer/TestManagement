using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestManagement.APP.Models.TestAnalysis
{
    public class StatusMaster
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property to related Request objects.
        [JsonIgnore]
        public ICollection<Request> Requests { get; set; } = new List<Request>();
    }
}
