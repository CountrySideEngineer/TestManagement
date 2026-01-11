namespace TestManagement.APP.Models
{
    public class TestRunDto
    {
        public int Id { get; set; }

        public DateTime ExecutedAt { get; set; }

        public string Environment { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;

        public ICollection<TestResultDto> TestResults { get; set; } = new List<TestResultDto>();
    }
}
