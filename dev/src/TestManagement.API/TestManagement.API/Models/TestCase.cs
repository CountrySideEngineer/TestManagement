using System.ComponentModel.DataAnnotations;

namespace TestManagement.API.Models
{
    /// <summary>
    /// Represents a test case which can have multiple versions.
    /// </summary>
    public class TestCase
    {
        /// <summary>
        /// Internal collection that stores versions of this test case.
        /// </summary>
        private readonly List<TestCaseVersion> _versions = new();

        /// <summary>
        /// Primary key identifier for the test case.
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Test case code to specify test case.
        /// It should be unique across the system and is used to identify the test case
        /// in a human-readable way. 
        /// It can be used for referencing the test case in documentation, reports,
        /// and when executing tests. The code should follow a consistent format
        /// to ensure clarity and ease of use.
        /// </summary>
        public string Code { get; set; } = null!;

        /// <summary>
        /// Indicates whether the test case is currently active.
        /// </summary>
        [Required]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Timestamp when the test case was created (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp when the test case was last updated (UTC).
        /// </summary>
        public DateTime UpdatedAt { get;set; } = DateTime.UtcNow;

        /// <summary>
        /// Read-only collection of versions associated with this test case.
        /// </summary>
        public IReadOnlyCollection<TestCaseVersion> Versions => _versions;

        /// <summary>
        /// Adds a new version for this test case.
        /// </summary>
        /// <param name="name">The name of the new version.</param>
        /// <param name="description">A description for the new version.</param>
        /// <param name="testLevelId">Identifier of the test level associated with the version.</param>
        /// <exception cref="InvalidOperationException">Thrown when the existing versions collection cannot be used to determine the next version number.</exception>
        public void AddVersion(string name, string description, int testLevelId)
        {
            TestCaseVersion newVersion = new TestCaseVersion
            {
                Name = name,
                Description = description,
                VersionNumber = 1,
                TestLevelId = testLevelId,
                TestCaseId = this.Id,
                IsLatest = true
            };

            if (_versions.Count < 1)
            {
                // If no version information is registered for the test case,
                // set the new test case version to 1 when registering it.
                newVersion.VersionNumber = 1;
            }
            else
            {
                try
                {
                    // The new version will be the current latest version plus 1.
                    var latestVersion = _versions.OrderByDescending(_ => _.VersionNumber).First();
                    newVersion.VersionNumber = latestVersion.VersionNumber + 1;
                }
                catch (Exception ex)
                when ((ex is ArgumentNullException) ||
                    (ex is InvalidOperationException))
                {
                    // If the version information cannot be retrieved, it may indicate a system error.
                    throw new InvalidOperationException(); // Re-throw a more specific exception if the versions collection is empty or null
                }
            }

            // Mark existing versions as not latest
            foreach (var version in _versions)
            {
                version.IsLatest = false;
            }
            _versions.Add(newVersion);
        }

    }
}
