using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestManagement.Data;
using TestManagement.Models;

namespace TestManagement.Pages.TestRuns
{
    public class IndexModel : PageModel
    {
        private readonly TestManagement.Data.AppDbContext _context;

        public IndexModel(TestManagement.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<TestRun> TestRun { get;set; } = default!;

        public async Task OnGetAsync()
        {
            TestRun = await _context.TestRuns
                .Include(t => t.TestCase)
                .OrderByDescending(tr => tr.ExecutedAt)
                .ToListAsync();
        }
    }
}
