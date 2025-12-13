using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestManagement.Data;
using TestManagement.Models;

namespace TestManagement.Pages.TestRuns
{
    public class CreateModel : PageModel
    {
        private readonly TestManagement.Data.AppDbContext _context;

        public CreateModel(TestManagement.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TestRun TestRun { get; set; } = new();

        [FromQuery(Name = "testCaseId")]
        public int? TestCaseId { get; set; }

        public IActionResult OnGet()
        {
            if (TestCaseId.HasValue)
            {
                TestRun = new TestRun
                {
                    TestCaseId = TestCaseId.Value,
                    ExecutedAt = DateTime.UtcNow
                };
            }
            return Page();
        }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            TestRun.TestCase = null;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            TestRun.ExecutedAt = DateTime.SpecifyKind(TestRun.ExecutedAt, DateTimeKind.Utc);
            _context.TestRuns.Add(TestRun);
            await _context.SaveChangesAsync();

            return RedirectToPage("./TestCases/details", new {id = TestRun.TestCaseId });
        }
    }
}
