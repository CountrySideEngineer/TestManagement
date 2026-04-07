namespace TestManagement.APP.ViewModel.Dashboard
{
    public class RecentExecutionViewModel
    {
        public bool HasFailure { get; init; } = false;

        public DateTime ExecutedAt { get; init; } = DateTime.UtcNow;

        public string Revision { get; init; } = string.Empty;
    }
}
