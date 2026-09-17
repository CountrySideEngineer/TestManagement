namespace TestManagement.API.Features.TestSuite.Create;

public class TestSuiteCompositionCreateRequest
{
    /// <summary>
    /// Identifier of the test case to be added to the test suite.
    /// </summary>
    public long TestCaseId { get; set; }

    /// <summary>
    /// The version number of the test case to add.
    /// </summary>
    public long TestCaseVersionNumber { get; set; }
}
