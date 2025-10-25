namespace TestManagement.Model
{
    public class TestResultTrendPoint
    {
        public DateTime Date { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public int SkippedCount { get; set; }
    }
}
