namespace TestManagement.API.Features.Project.Create;

/// <summary>
/// Request DTO used to create a new project.
/// Contains the properties required by the API to create a project resource.
/// </summary>
public class CreateProjectRequest
{
    /// <summary>
    /// The display name of the project.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Optional description providing additional information about the project.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}
