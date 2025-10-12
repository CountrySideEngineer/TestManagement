using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestManagement.Data;
using TestManagement.Models;

namespace TestManagement.Controllers
{
    public class TestRunsController : Controller
    {
        private readonly AppDbContext _context;

        public TestRunsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: TestRuns
        public async Task<IActionResult> Index()
        {
            var testRuns = await _context.TestRuns
                .Include(t => t.TestCase)
                .ToListAsync();
            return View(testRuns);
        }

        // GET: TestRuns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testRun = await _context.TestRuns
                .Include(t => t.TestCase)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testRun == null)
            {
                return NotFound();
            }

            return View(testRun);
        }

        // GET: TestRuns/Create
        public IActionResult Create()
        {
            ViewData["TestCaseId"] = new SelectList(_context.TestCases, "Id", "Name");
            return View();
        }

        // POST: TestRuns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TestCaseId,Status,Remarks,ExecutedAt")] TestRun testRun)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testRun);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TestCaseId"] = new SelectList(_context.TestCases, "Id", "Name", testRun.TestCaseId);
            return View(testRun);
        }

        // GET: TestRuns/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testRun = await _context.TestRuns.FindAsync(id);
            if (testRun == null)
            {
                return NotFound();
            }
            ViewData["TestCaseId"] = new SelectList(_context.TestCases, "Id", "Name", testRun.TestCaseId);
            return View(testRun);
        }

        // POST: TestRuns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TestCaseId,Status,Remarks,ExecutedAt")] TestRun testRun)
        {
            if (id != testRun.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testRun);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestRunExists(testRun.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TestCaseId"] = new SelectList(_context.TestCases, "Id", "Name", testRun.TestCaseId);
            return View(testRun);
        }

        // GET: TestRuns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testRun = await _context.TestRuns
                .Include(t => t.TestCase)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testRun == null)
            {
                return NotFound();
            }

            return View(testRun);
        }

        // POST: TestRuns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testRun = await _context.TestRuns.FindAsync(id);
            if (testRun != null)
            {
                _context.TestRuns.Remove(testRun);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestRunExists(int id)
        {
            return _context.TestRuns.Any(e => e.Id == id);
        }
    }
}
