using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestManagement.Data;
using TestManagement.Models;

namespace TestManagement.Pages.TestCases
{
    public class DetailsModel : PageModel
    {
        private readonly TestManagement.Data.AppDbContext _context;

        public DetailsModel(TestManagement.Data.AppDbContext context)
        {
            _context = context;
        }

        public TestCase TestCase { get; set; } = default!;

        public List<TestRun> TestRuns { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testcase = await _context.TestCases
                .Include(tc => tc.TestRuns)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testcase == null)
            {
                return NotFound();
            }
            else
            {
                TestCase = testcase;
                TestRuns = await _context.TestRuns.Where(tr => tr.TestCaseId == testcase.Id)
                    .OrderByDescending(tr => tr.ExecutedAt)
                    .ToListAsync();
            }
            return Page();
        }
    }
}
