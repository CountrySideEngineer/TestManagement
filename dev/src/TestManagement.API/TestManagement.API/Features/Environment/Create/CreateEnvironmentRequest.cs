namespace TestManagement.API.Features.Environment.Create
{
    public class CreateEnvironmentRequest
    {
        public string Name { get; set; } = string.Empty;

        public string Os { get; set; } = string.Empty;

        public string RunTime { get; set; } = string.Empty;
    }
}
