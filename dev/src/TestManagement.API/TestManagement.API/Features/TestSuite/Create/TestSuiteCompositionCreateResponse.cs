namespace TestManagement.API.Features.TestSuite.Create;

public class TestSuiteCompositionCreateResponse
{
    /// <summary>
    /// Primary key identifier of the newly created or existing test suite composition record.
    /// </summary>
    public long TestSuiteCompositionId { get; set; }

    /// <summary>
    /// Identifier of the test suite that contains the test case version.
    /// </summary>
    public long TestSuiteId { get; set; }

    /// <summary>
    /// Identifier of the test case that is part of the composition.
    /// </summary>
    public long TestCaseId { get; set; }

    /// <summary>
    /// Version number of the test case that was added to the test suite.
    /// </summary>
    public long TestCaseVersionNumber { get; set; }
}
