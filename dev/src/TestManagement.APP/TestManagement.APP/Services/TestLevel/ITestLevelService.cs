using TestManagement.APP.ViewModel.TestLevel;

namespace TestManagement.APP.Services.TestLevel
{
    public interface ITestLevelService
    {
        Task<ICollection<TestLevelViewModel>> GetTestLevelAsync();
    }
}
