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
    public class DetailsModel : PageModel
    {
        private readonly TestManagement.Data.AppDbContext _context;

        public DetailsModel(TestManagement.Data.AppDbContext context)
        {
            _context = context;
        }

        public TestRun TestRun { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testrun = await _context.TestRuns.FirstOrDefaultAsync(m => m.Id == id);
            if (testrun == null)
            {
                return NotFound();
            }
            else
            {
                TestRun = testrun;
            }
            return Page();
        }
    }
}
