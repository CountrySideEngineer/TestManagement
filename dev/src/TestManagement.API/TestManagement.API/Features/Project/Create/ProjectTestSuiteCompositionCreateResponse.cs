namespace TestManagement.API.Features.Project.Create;

/// <summary>
/// Represents the response returned after creating a project and test suite composition.
/// </summary>
public class ProjectTestSuiteCompositionCreateResponse
{
    /// <summary>
    /// Gets or sets the identifier of the created composition.
    /// </summary>
    public long CompositionId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the test suite linked to the project.
    /// </summary>
    public long TestSuiteId { get; set; }
}
