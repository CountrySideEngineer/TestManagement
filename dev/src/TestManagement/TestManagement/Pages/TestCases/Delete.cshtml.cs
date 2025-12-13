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
    public class DeleteModel : PageModel
    {
        private readonly TestManagement.Data.AppDbContext _context;

        public DeleteModel(TestManagement.Data.AppDbContext context)
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

            var testcase = await _context.TestCases.FirstOrDefaultAsync(m => m.Id == id);

            if (testcase == null)
            {
                return NotFound();
            }
            else
            {
                TestCase = testcase;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testcase = await _context.TestCases.FindAsync(id);
            if (testcase != null)
            {
                TestCase = testcase;
                _context.TestCases.Remove(TestCase);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
