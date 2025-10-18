using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestManagement.Data;
using TestManagement.Models;

namespace TestManagement.Pages.TestRuns
{
    public class EditModel : PageModel
    {
        private readonly TestManagement.Data.AppDbContext _context;

        public EditModel(TestManagement.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TestRun TestRun { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testrun =  await _context.TestRuns.FirstOrDefaultAsync(m => m.Id == id);
            if (testrun == null)
            {
                return NotFound();
            }
            TestRun = testrun;
           ViewData["TestCaseId"] = new SelectList(_context.TestCases, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            TestRun.ExecutedAt = DateTime.SpecifyKind(TestRun.ExecutedAt, DateTimeKind.Utc);

            _context.Attach(TestRun).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestRunExists(TestRun.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TestRunExists(int id)
        {
            return _context.TestRuns.Any(e => e.Id == id);
        }
    }
}
