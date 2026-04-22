using TestManagement.APP.ViewModel.Environment;

namespace TestManagement.APP.ViewModel.Executions
{
    public class ExecutionCreateViewModel
    {
        public IList<EnvironmentModel> Environments { get; set; } = new List<EnvironmentModel>();

        public ExecutionCreateViewModel() { }
    }
}
