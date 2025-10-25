using Microsoft.EntityFrameworkCore;

namespace TestManagement.Data
{
    public class TestManagementDbContext : DbContext
    {
        public TestManagementDbContext(DbContextOptions<TestManagementDbContext> options)
            : base(options)
        {
        }
    }
}
