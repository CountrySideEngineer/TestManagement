namespace TestManagement.Model
{
    public class ProjectSummary
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = "";
        public int TotalCases { get; set; }
        public int ExecutedCases { get; set; }
        public double SuccessRate { get; set; }
        public DateTime? LastExecutedAt { get; set; }
    }
}
