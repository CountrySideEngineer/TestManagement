namespace TestManagement.API.Features.TestCases.Create
{
    public class CreateTestCaseResponse
    {
        public int Id { get; set; } = 0;

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int TestLevelId { get; set; } = 0;

        public int VersionNumber { get; set; } = 0;
    }
}
