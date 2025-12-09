using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManagement.Analyze.APP.Model.DTO
{
    internal class TestResultDto
    {
        public int Id { get; set; }

        public string ActualResult { get; set; } = string.Empty;

        public int TestCaseId { get; set; }
        public TestCaseDto TestCase { get; set; } = new ();

        public int TestRunId { get; set; }
        public TestRunDto TestRun { get; set; } = new ();


        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
