namespace TestManagement.Model
{
    public class TestRunSummary
    {
        public int RunId { get; set; }
        public DateTime ExecutedAt { get; set; }
        public string Executor { get; set; } = "";
        public string CaseName { get; set; } = "";
        public string Result { get; set; } = "";
    }
}
