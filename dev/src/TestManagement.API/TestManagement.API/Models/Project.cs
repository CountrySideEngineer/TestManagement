namespace TestManagement.API.Models
{
    public class Project
    {
        public long Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        private readonly List<TestSuite> _testSuites = new();

        public IReadOnlyCollection<TestSuite> TestSuites => _testSuites;

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
                if (testSuite.ProjectId != 0)
                {
                    if (_testSuites.Any(ts => ts.ProjectId == testSuite.ProjectId && string.Equals(ts.Name, testSuite.Name, StringComparison.Ordinal)))
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
            }

            testSuite.Project = this;
            testSuite.ProjectId = this.Id;

            _testSuites.Add(testSuite);

            UpdatedAt = DateTime.UtcNow;
            return true;
        }
    }
}
