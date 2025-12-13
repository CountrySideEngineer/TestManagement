using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestManagement.Data;
using TestManagement.Models;

namespace TestManagement.Controllers
{
    public class TestCasesController : Controller
    {
        private readonly AppDbContext _context;

        public TestCasesController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var testCases = await _context.TestCases
                .Include(tc => tc.TestRuns)
                .ToListAsync();
            return View(testCases);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TestCase testCase)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testCase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(testCase);
        }

        public async Task<IActionResult> Details(int id)
        {
            var testCase = await _context.TestCases
                .Include(tc => tc.TestRuns)
                .FirstOrDefaultAsync(tc => tc.Id == id);

            if (testCase == null)
                return NotFound();

            return View(testCase);
        }
    }
}
