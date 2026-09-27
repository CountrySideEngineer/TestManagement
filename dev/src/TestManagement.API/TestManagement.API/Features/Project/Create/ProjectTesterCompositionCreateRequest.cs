namespace TestManagement.API.Features.Project.Create;

/// <summary>
/// Request containing the tester to associate with a project.
/// </summary>
public class ProjectTesterCompositionCreateRequest
{
    /// <summary>
    /// Gets or sets the identifier of the tester to associate with the project.
    /// </summary>
    public long TesterId { get; set; }
}
