namespace TestManagement.API.Features.Environment.Create
{
    public class CreateEnvironmentResponse
    {
        public long Id { get; set; } = 0;

        public string Name { get; set; } = string.Empty;

        public string Os { get; set; } = string.Empty;

        public string RunTime { get; set; } = string.Empty;

        public long VersionNumber { get; set; } = 0;
    }
}
