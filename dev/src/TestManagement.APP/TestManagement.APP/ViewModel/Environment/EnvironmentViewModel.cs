namespace TestManagement.APP.ViewModel.Environment
{
    /// <summary>
    /// Represents a deployment or test environment with identifying information
    /// such as operating system and runtime. This view model is used to
    /// populate environment selection lists in the UI.
    /// </summary>
    public class EnvironmentViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier of the environment.
        /// </summary>
        public long EnvironmentId { get; set; } = 0;

        /// <summary>
        /// Gets or sets the internal name of the environment.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the operating system string for the environment (e.g. "Windows", "Linux").
        /// </summary>
        public string Os { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the runtime or framework version used in the environment (e.g. "dotnet 8").
        /// </summary>
        public string RunTime { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the human-friendly display name for the environment.
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentViewModel"/> class.
        /// </summary>
        public EnvironmentViewModel() { }
    }
}
