namespace TestManagement.Model
{
    public class TestResultStatistics
    {
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public int SkippedCount { get; set; }
        public double SuccessRate => (SuccessCount + FailureCount + SkippedCount) == 0
            ? 0
            : 100.0 * SuccessCount / (SuccessCount + FailureCount + SkippedCount);
    }
}
