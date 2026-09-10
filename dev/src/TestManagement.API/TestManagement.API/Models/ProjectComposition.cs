using System.Text.Json.Serialization;

namespace TestManagement.API.Models;

/// <summary>
/// Join entity that associates <see cref="Project"/> and <see cref="TestSuite"/> (N:N).
/// </summary>
public class ProjectComposition
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
    /// Foreign key referencing the associated <see cref="TestSuite"/>.
    /// </summary>
    public long TestSuiteId { get; set; }

    /// <summary>
    /// Navigation property to the <see cref="Project"/>. May be null for detached or DTO instances.
    /// </summary>
    [JsonIgnore]
    public virtual Project? Project { get; set; }

    /// <summary>
    /// Navigation property to the <see cref="TestSuite"/>. May be null for detached or DTO instances.
    /// </summary>
    [JsonIgnore]
    public virtual TestSuite? TestSuite { get; set; }
}
