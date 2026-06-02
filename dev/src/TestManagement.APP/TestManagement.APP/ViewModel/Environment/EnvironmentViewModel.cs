namespace TestManagement.APP.ViewModel.Environment
{
    public class EnvironmentViewModel
    {
        public long EnvironmentId { get; set; } = 0;

        public string Name { get; set; } = string.Empty;

        public string Os { get; set; } = string.Empty;

        public string RunTime { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public EnvironmentViewModel() { }
    }
}
