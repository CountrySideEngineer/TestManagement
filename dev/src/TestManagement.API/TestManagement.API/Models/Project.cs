using System;
using System.Collections.Generic;
using System.Linq;

namespace TestManagement.API.Models
{
    /// <summary>
    /// Represents a project that can contain multiple test suites and test executions.
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Gets or sets the primary key identifier for the project.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the project.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the UTC timestamp when the project was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the UTC timestamp when the project was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Stores the test suites associated with this project.
        /// The internal list preserves the insertion order of the suites.
        /// </summary>
        private readonly List<TestSuite> _testSuites = new();

        /// <summary>
        /// Gets a read-only collection of test suites associated with this project.
        /// </summary>
        public IReadOnlyCollection<TestSuite> TestSuites => _testSuites;

        /// <summary>
        /// Stores the test executions associated with this project.
        /// </summary>
        private readonly List<TestExecution> _executions = new();

        /// <summary>
        /// Gets a read-only collection of test executions associated with this project.
        /// </summary>
        public IReadOnlyCollection<TestExecution> Executions => _executions;

        /// <summary>
        /// Attempts to add a test suite to this project.
        /// </summary>
        /// <remarks>
        /// Persisted suites are considered duplicates when another suite has the same identifier.
        /// Non-persisted suites are considered duplicates when the same instance is already associated
        /// with this project.
        /// When the suite is added successfully, its project association is set to this project
        /// and the project's update timestamp is refreshed.
        /// </remarks>
        /// <param name="testSuite">
        /// The test suite to add. This value must not be <see langword="null"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the test suite was added;
        /// otherwise, <see langword="false"/> when the suite is a duplicate.
        /// </returns>
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

        /// <summary>
        /// Adds a new test execution to this project.
        /// </summary>
        /// <param name="execution">
        /// The test execution to add. This value must not be <see langword="null"/>.
        /// The execution identifier must be zero because the execution has not been persisted yet.
        /// </param>
        /// <returns>
        /// <see langword="true"/> after the test execution has been added successfully.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="execution"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the execution already has a non-zero identifier.
        /// </exception>
        public virtual bool AddTestExecution(TestExecution execution)
        {
            ArgumentNullException.ThrowIfNull(execution);

            if (0 != execution.Id)
            {
                throw new ArgumentException(
                    "Execution Id must be 0 when adding a new execution to a project.",
                    nameof(execution));
            }

            execution.ProjectId = Id;
            execution.Project = this;

            _executions.Add(execution);

            UpdatedAt = DateTime.UtcNow;

            return true;
        }
    }
}