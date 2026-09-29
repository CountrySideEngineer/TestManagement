namespace TestManagement.API.Features.TestSuite.Get;

/// <summary>
/// Response DTO used when returning a test suite along with a collection of
/// summarized test case versions included in that suite.
/// </summary>
public class GetTestSuiteResponse
{
    /// <summary>
    /// Primary key identifier for the test suite.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Name of the test suite.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the test suite.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}
