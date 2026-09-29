using TestManagement.API.Features.Tester.Get;
using TestManagement.API.Features.TestSuite.Get;

namespace TestManagement.API.Features.Project.Get;

public class GetProjectWithTestSuiteResponse
{
    /// <summary>
    /// The unique identifier of the project.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// The display name of the project.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// A short description of the project.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    public List<TestSuiteSummary>? TestSuiteSummaries { get; set; } = null;

    public class TestSuiteSummary
    {
        public long Id { get; set; } = 0;

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
