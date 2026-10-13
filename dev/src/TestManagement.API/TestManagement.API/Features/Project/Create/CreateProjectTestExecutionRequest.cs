namespace TestManagement.API.Features.Project.Create;

/// <summary>
/// Represents the request data required to create a project test execution.
/// </summary>
public class CreateProjectTestExecutionRequest
{
    /// <summary>
    /// The UTC date and time when the test execution occurred.
    /// Defaults to the current UTC time if not provided.
    /// </summary>
    public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The target environment for the test execution (e.g. "staging", "production").
    /// </summary>
    public string Environment { get; set; } = null!;

    /// <summary>
    /// The revision identifier (commit hash or build number) associated with the execution.
    /// </summary>
    public string Revision { get; set; } = null!;

    /// <summary>
    /// The identifier of the project associated with the test execution.
    /// </summary>
    public long ProjectId { get; set; } = 0;
}
