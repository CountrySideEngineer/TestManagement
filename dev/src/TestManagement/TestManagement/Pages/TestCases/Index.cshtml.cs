using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestManagement.Data;
using TestManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestManagement.Pages.TestCases
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<TestCase> TestCases { get; set; } = new List<TestCase>();

        public async Task OnGetAsync()
        {
            TestCases = await _context.TestCases
                .Include(tc => tc.TestRuns)
                .ToListAsync();
        }
    }
}
