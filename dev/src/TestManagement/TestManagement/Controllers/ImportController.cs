using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using TestManagement.Data;
using TestManagement.Models;

namespace TestManagement.Controllers
{
    public class ImportController : Controller
    {
        private readonly AppDbContext _context;

        public ImportController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Upload(IFormFile xmlFile)
        {
            var history = new XmlImportHistory()
            {
                FileName = xmlFile.FileName,
                ImportedAt = DateTime.Now
            };

            try
            {
                using var stream = xmlFile.OpenReadStream();
                var doc = XDocument.Load(stream);

                foreach (var testCaseNode in doc.Descendants("testcase"))
                {
                    var name = (string)testCaseNode.Attribute("name");
                    var status = (string)testCaseNode.Attribute("status");

                    var testCase = _context.TestCases.FirstOrDefault(_ => _.Name == name);
                    if (null == testCase)
                    {
                        testCase = new TestCase()
                        {
                            Name = name,
                            CreatedAt = DateTime.Now
                        };
                        _context.TestCases.Add(testCase);
                        _context.SaveChanges();
                    }

                    var run = new TestRun()
                    {
                        TestCaseId = testCase.Id,
                        Status = status,
                        ExecutedAt = DateTime.Now
                    };
                    _context.TestRuns.Add(run);
                }
                _context.SaveChanges();
                history.Status = "Success";
            }
            catch (Exception ex)
            {
                history.Status = "Error";
                history.ErrorMessage = ex.Message;
            }

            _context.XmlImportHistories.Add(history);
            _context.SaveChanges();

            return RedirectToAction("Index", "TestRun");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
