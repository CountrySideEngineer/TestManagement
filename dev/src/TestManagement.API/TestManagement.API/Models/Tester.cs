using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TestManagement.API.Models;

/// <summary>
/// Represents a tester associated with a project.
/// Maps to the Testers table.
/// </summary>
public class Tester
{
    /// <summary>
    /// Primary key identifier for the tester.
    /// </summary>
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// Tester name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Email address of the tester.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// UTC timestamp when the tester record was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// UTC timestamp when the tester record was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
