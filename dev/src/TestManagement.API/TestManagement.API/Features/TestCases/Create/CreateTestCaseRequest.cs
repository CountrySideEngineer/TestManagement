namespace TestManagement.API.Features.TestCases.Create
{
    public class CreateTestCaseRequest
    {
        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int TestLevelId { get; set; } = 0;
    }
}
