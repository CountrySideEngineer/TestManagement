using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestManagement.Data;
using TestManagement.Models;

namespace TestManagement.Pages.TestCases
{
    public class EditModel : PageModel
    {
        private readonly TestManagement.Data.AppDbContext _context;

        public EditModel(TestManagement.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TestCase TestCase { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testcase =  await _context.TestCases.FirstOrDefaultAsync(m => m.Id == id);
            if (testcase == null)
            {
                return NotFound();
            }
            TestCase = testcase;
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

            TestCase.UpdatedAt = DateTime.UtcNow;

            // Set changes as tracking.
            _context.Attach(TestCase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestCaseExists(TestCase.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Redirect to index page after saving changes.
            return RedirectToPage("./Index");
        }

        private bool TestCaseExists(int id)
        {
            return _context.TestCases.Any(e => e.Id == id);
        }
    }
}
