namespace TestManagement.APP.ViewModel.Dashboard
{
    public class RecentExecutionViewModel
    {
        /// <summary>
        /// Indicates whether the execution contained any failures.
        /// </summary>
        public bool HasFailure { get; init; } = false;

        /// <summary>
        /// The UTC date and time when the execution was performed.
        /// </summary>
        public DateTime ExecutedAt { get; init; } = DateTime.UtcNow;

        /// <summary>
        /// The source control revision identifier for this execution.
        /// </summary>
        public string Revision { get; init; } = string.Empty;
    }
}
