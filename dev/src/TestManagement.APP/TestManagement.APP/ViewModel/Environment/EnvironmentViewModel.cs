namespace TestManagement.APP.ViewModel.Environment
{
    /// <summary>
    /// View model representing an environment used by the application UI.
    /// Contains basic identifying fields and a pre-formatted display string.
    /// </summary>
    public class EnvironmentViewModel
    {
        /// <summary>
        /// The unique identifier of the environment record.
        /// </summary>
        public long EnvironmentId { get; set; } = 0;

        /// <summary>
        /// The name of the environment.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The operating system name for the environment.
        /// </summary>
        public string Os { get; set; } = string.Empty;

        /// <summary>
        /// The runtime version string associated with the environment.
        /// </summary>
        public string RunTime { get; set; } = string.Empty;

        /// <summary>
        /// A human-friendly string composed from the other properties for display in UI lists.
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public EnvironmentViewModel() { }
    }
}
