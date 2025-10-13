using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestManagement.Models;

namespace TestManagement.Pages.TestCases
{
    public class CreateModel : PageModel
    {
        private readonly TestManagement.Data.AppDbContext _context;

        public CreateModel(TestManagement.Data.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public TestCase TestCase { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // 作成日時をUTCで設定
            TestCase.CreatedAt = DateTime.UtcNow;

            _context.TestCases.Add(TestCase);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
