using TestManagement.APP.ViewModel.Environment;

namespace TestManagement.APP.ViewModel.Executions
{
    public class ExecutionCreateViewModel
    {
        public IList<EnvironmentViewModel> Environments { get; set; } = new List<EnvironmentViewModel>();

        public ExecutionCreateViewModel() { }
    }
}
