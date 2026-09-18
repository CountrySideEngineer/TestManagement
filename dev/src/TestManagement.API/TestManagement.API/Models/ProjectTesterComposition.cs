using System.Text.Json.Serialization;

namespace TestManagement.API.Models;

/// <summary>
/// Join entity that associates <see cref="Project"/> and <see cref="Tester"/> (N:N).
/// </summary>
public class ProjectTesterComposition
{
    /// <summary>
    /// Primary key identifier for the composition.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Foreign key referencing the owning <see cref="Project"/>.
    /// </summary>
    public long ProjectId { get; set; }

    /// <summary>
    /// Foreign key referencing the associated <see cref="Tester"/>.
    /// </summary>
    public long TesterId { get; set; }

    /// <summary>
    /// Navigation property to the <see cref="Project"/>. May be null for detached or DTO instances.
    /// </summary>
    [JsonIgnore]
    public virtual Project? Project { get; set; }

    /// <summary>
    /// Navigation property to the <see cref="Tester"/>. May be null for detached or DTO instances.
    /// </summary>
    [JsonIgnore]
    public virtual Tester? Tester { get; set; }

    /// <summary>
    /// UTC timestamp when this composition was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// UTC timestamp when this composition was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
