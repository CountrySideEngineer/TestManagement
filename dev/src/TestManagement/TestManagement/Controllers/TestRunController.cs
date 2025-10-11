using Microsoft.AspNetCore.Mvc;
using TestManagement.Data;
using TestManagement.Models;

namespace TestManagement.Controllers
{
    public class TestRunController : Controller
    {
        private readonly AppDbContext _context;

        public TestRunController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            ViewBag.TestCases = _context.TestCases.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(int testCaseId, string status, string log)
        {
            var run = new TestRun
            {
                TestCaseId = testCaseId,
                Status = status,
                Log = log,
                ExecutedAt = DateTime.Now
            };
            _context.TestRuns.Add(run);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            var runs = _context.TestRuns
                .OrderByDescending(_ => _.ExecutedAt)
                .ToList();
            return View(runs);
        }
    }
}
