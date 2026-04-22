namespace TestManagement.APP.Dto.Environment.Get
{
    public class GetEnvironmentResponse
    {
        public long EnvironmentId { get; set; } = 0;

        public string Name { get; set; } = string.Empty;

        public string Os { get;set; } = string.Empty;

        public string RunTime { get; set; } = string.Empty;

        public long Version { get; set; } = 0;
    }
}
