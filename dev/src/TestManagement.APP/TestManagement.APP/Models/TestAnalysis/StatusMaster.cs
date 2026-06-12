using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestManagement.APP.Models.TestAnalysis
{
    public class StatusMaster
    {
        /// <summary>
        /// Primary identifier for the status master record.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Name of the status (e.g. "Open", "InProgress").
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// UTC timestamp when the status master record was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// UTC timestamp when the status master record was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Navigation property containing related <see cref="Request"/> entities.
        /// This collection is ignored during JSON serialization to avoid cycles.
        /// </summary>
        [JsonIgnore]
        public ICollection<Request> Requests { get; set; } = new List<Request>();
    }
}
