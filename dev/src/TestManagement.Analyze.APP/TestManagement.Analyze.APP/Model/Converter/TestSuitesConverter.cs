using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestManagement.Analyze.APP.Model.DTO;
using TestManagement.Analyze.APP.Model.Xml;

namespace TestManagement.Analyze.APP.Model.Converter
{
    internal class TestSuitesConverter
    {
        internal class TestCaseBuilder
        {
            private readonly string _testSuiteName = string.Empty;

            public TestCaseBuilder(string testSuiteName)
            {
                _testSuiteName = testSuiteName;
            }

            public string MakeTestCaseName(string testName)
            {
                string testCaseName = $"{_testSuiteName}::{testName}";

                return testCaseName;
            }
        }

        public IEnumerable<TestCaseDto> ToTestCases(IEnumerable<TestSuites> testSuitesCollection)
        {
            var testCases = new List<TestCaseDto>();
            foreach (var suitesItem in testSuitesCollection)
            {
                var newTestCases = ToTestCases(suitesItem.TestItems);
                testCases.AddRange(newTestCases);
            }
            return testCases;
        }

        public IEnumerable<TestCaseDto> ToTestCases(IEnumerable<TestSuite> testSuiteCollection)
        {
            var testCases = new List<TestCaseDto>();
            foreach (var testSuite in testSuiteCollection)
            {
                var  newTestCases = ToTestCases(testSuite);
                testCases.AddRange(newTestCases);
            }

            return testCases;
        }

        public IEnumerable<TestCaseDto> ToTestCases(TestSuite testSuite)
        {
            var builder = new TestCaseBuilder(testSuite.Name);
            foreach (var testCase in testSuite.TestCases)
            {
                string testCaseName = builder.MakeTestCaseName(testCase.Name);
                var testCaseDto = new TestCaseDto()
                {
                    Title = testCaseName,
                    Description = testCase.Name,
                    TestLevelId = 1
                };
                yield return testCaseDto;
            }
        }
    }
}
