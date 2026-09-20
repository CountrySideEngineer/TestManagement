using System;
using System.Collections.Generic;
using System.Linq;

namespace TestManagement.API.Models
{
    /// <summary>
    /// Represents a project which can contain multiple test suites.
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Primary key identifier for the project.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Project name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Project description.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// UTC timestamp when the project was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// UTC timestamp when the project was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Backing collection that holds the test suites for this project.
        /// Internal implementation uses a List to preserve insertion order.
        /// </summary>
        private readonly List<TestSuite> _testSuites = new();

        /// <summary>
        /// Read-only view of the test suites associated with this project.
        /// </summary>
        public IReadOnlyCollection<TestSuite> TestSuites => _testSuites;

        /// <summary>
        /// Attempts to add the given <see cref="TestSuite"/> to this project.
        /// If the suite is considered a duplicate (by Id when persisted, or by ProjectId+Name for non-persisted),
        /// the method does not add it and returns false.
        /// On successful addition the method sets the suite's Project and ProjectId to this project and updates <see cref="UpdatedAt"/>.
        /// </summary>
        /// <param name="testSuite">The test suite to add. Must not be null.</param>
        /// <returns>True if the suite was added; false if it was detected as a duplicate and not added.</returns>
        public bool AddTestSuite(TestSuite testSuite)
        {
            ArgumentNullException.ThrowIfNull(testSuite);

            if (testSuite.Id != 0)
            {
                if (_testSuites.Any(ts => ts.Id == testSuite.Id))
                {
                    return false;
                }
            }
            else
            {
                if (_testSuites.Contains(testSuite))
                {
                    return false;
                }
            }

            testSuite.Project = this;

            _testSuites.Add(testSuite);

            UpdatedAt = DateTime.UtcNow;
            return true;
        }
    }
}
