namespace TestManagement.API.Models;

/// <summary>
/// Represents a test execution that groups together test execution items
/// run in a specific environment and project.
/// </summary>
public class TestExecution
{
    /// <summary>
    /// Gets or sets the unique identifier of the test execution.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the revision or version identifier associated with the execution,
    /// such as a commit hash or build number.
    /// </summary>
    public string Revision { get; set; } = null!;

    /// <summary>
    /// Gets or sets the identifier of the project associated with the test execution.
    /// </summary>
    public long ProjectId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when this record was created, in UTC.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the date and time when this record was last updated, in UTC.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the project associated with this test execution.
    /// </summary>
    public Project Project { get; set; } = null!;

    /// <summary>
    /// Stores the collection of test execution items associated with this execution.
    /// </summary>
    private readonly List<TestExecutionItem> _items = new();

    /// <summary>
    /// Gets the read-only collection of test execution items associated with this execution.
    /// </summary>
    public IReadOnlyCollection<TestExecutionItem> Items => _items;

    /// <summary>
    /// Represents the input data required to create a test execution item.
    /// </summary>
    public class TestExecutionItemInner
    {
        /// <summary>
        /// Gets or sets the identifier of the environment where the tests were executed.
        /// </summary>
        public long EnvId { get; set; } = 0;

        /// <summary>
        /// Gets or sets the date and time when the tests were executed, in UTC.
        /// </summary>
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the collection of tests executed as part of the execution item.
        /// </summary>
        public List<ExecutedTest> ExecutedTests { get; set; } = new List<ExecutedTest>();
    }

    /// <summary>
    /// Represents a test case and its status within a test execution.
    /// </summary>
    public class ExecutedTest
    {
        /// <summary>
        /// Gets or sets the identifier of the test case version.
        /// </summary>
        public long TestCaseVersionId { get; set; } = 0;

        /// <summary>
        /// Gets or sets the identifier of the test status.
        /// </summary>
        public long TestStatusId { get; set; } = 0;
    }

    /// <summary>
    /// Creates and adds a test execution item to this test execution.
    /// </summary>
    /// <param name="envId">
    /// The identifier of the environment where the tests were executed.
    /// </param>
    /// <param name="executedAt">
    /// The date and time when the tests were executed.
    /// </param>
    /// <param name="executedTests">
    /// The collection of tests executed during the execution item.
    /// </param>
    public void AddExecutionItem(
        long envId,
        DateTime executedAt,
        ICollection<ExecutedTest> executedTests)
    {
        var newItem = new TestExecutionItem
        {
            EnvironmentId = envId,
            ExecutedAt = executedAt,
            TestResults = executedTests.Select(et => new TestResult
            {
                TestCaseVersionId = et.TestCaseVersionId,
                StatusId = et.TestStatusId,
                ExecutedAt = executedAt
            }).ToList()
        };

        _items.Add(newItem);
    }
}
