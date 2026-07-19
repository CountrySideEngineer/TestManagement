using TestManagement.APP.ViewModel.Environment;

namespace TestManagement.APP.ViewModel.Executions
{
    /// <summary>
    /// View model used when creating a new test execution. Contains the list of
    /// available environments that the execution can target.
    /// </summary>
    public class ExecutionCreateViewModel
    {
        /// <summary>
        /// Gets or sets the list of environments that can be selected for the
        /// execution. Each item describes a target environment (OS, runtime, etc.).
        /// </summary>
        public IList<EnvironmentViewModel> Environments { get; set; } = new List<EnvironmentViewModel>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionCreateViewModel"/> class.
        /// </summary>
        public ExecutionCreateViewModel() { }
    }
}
