namespace TestManagement.API.Features.Environment.Update
{
    public class UpdateEnvironmentResponse
    {
        public string Name { get; set; } = string.Empty;

        public string Os { get; set; } = string.Empty;

        public string RunTime { get; set; } = string.Empty;

        public long VersionNumber { get; set; } = 0;
    }
}
