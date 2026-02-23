using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using TestManagement.APP.Models;
using TestManagement.APP.Services.Option;

namespace TestManagement.APP.Services
{
    public class UploadFileParser
    {
        public async Task<IList<TestResultDto>> ParseAsync(IEnumerable<IFormFile> files, ParseOption option)
        {
            var results = new List<TestResultDto>();

            foreach (var file in files)
            {
                try
                {
                    using var stream = file.OpenReadStream();
                    using var sr = new StreamReader(stream);
                    var content = await sr.ReadToEndAsync();

                    if (string.IsNullOrWhiteSpace(content))
                    {
                        continue;
                    }

                    XDocument xdoc;
                    try
                    {
                        xdoc = XDocument.Parse(content);
                    }
                    catch
                    {
                        // Not a valid XML document - skip this file
                        continue;
                    }

                    var testcases = xdoc.Descendants("testcase");
                    foreach (var tc in testcases)
                    {
                        string name = (string?)tc.Attribute("name") ?? string.Empty;
                        string classname = (string?)tc.Attribute("classname") ?? string.Empty;

                        DateTime executedAt = DateTime.UtcNow;
                        var tsAttr = (string?)tc.Attribute("timestamp");
                        if (!string.IsNullOrEmpty(tsAttr) && DateTime.TryParse(tsAttr, out var parsed))
                        {
                            executedAt = DateTime.SpecifyKind(parsed, DateTimeKind.Utc);
                        }

                        bool hasFailure = tc.Element("failure") != null;

                        var testCaseDto = new TestCaseDto
                        {
                            Title = string.IsNullOrEmpty(classname) ? name : $"{classname}::{name}",
                            Description = name,
                            TestLevelId = option.TestLevelId ?? 0
                        };

                        var tr = new TestResultDto
                        {
                            ActualResult = hasFailure ? "FAIL" : "SUCCESS",
                            TestCase = testCaseDto,
                            ExecutedAt = executedAt,
                            Status = hasFailure ? TestStatus.Failure : TestStatus.Success,
                            TestRunId = option.RevisionId ?? 0
                        };

                        results.Add(tr);
                    }
                }
                catch
                {
                    // ignore individual file errors and continue with others
                }
            }

            return results;
        }
    }
}
