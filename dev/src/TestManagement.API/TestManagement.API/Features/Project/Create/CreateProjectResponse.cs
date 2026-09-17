namespace TestManagement.API.Features.Project.Create;

/// <summary>
/// Response DTO returned after a project is created.
/// Carries the persisted identifier along with the project properties.
/// </summary>
public class CreateProjectResponse
{
    /// <summary>
    /// The unique identifier assigned to the created project.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// The display name of the project.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description text for the project.
    /// </summary>
    public string Description { get; set; } = string.Empty; 
}
