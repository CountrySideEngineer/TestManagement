namespace TestManagement.API.Features.Environment.Get
{
    public class GetEnvironmentRequest
    {
        public string? Name { get; set; } = null!;

        public string? Os { get; set; } = null!;

        public string? RunTime { get; set; } = null!;
    }
}
