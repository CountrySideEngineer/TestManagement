namespace TestManagement.API.Features.Project.Create;

/// <summary>
/// Represents the response returned after creating a project and tester composition.
/// </summary>
public class ProjectTesterCompositionCreateResponse
{
    /// <summary>
    /// Gets or sets the identifier of the created composition.
    /// </summary>
    public long CompositionId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the tester linked to the project.
    /// </summary>
    public long TesterId { get; set; }
}
