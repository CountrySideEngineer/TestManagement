using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
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
                yield return ToTestCase(testCase, builder);
            }
        }

        public TestCaseDto ToTestCase(TestCase testCase, TestCaseBuilder builder)
        {
            string testCaseName = builder.MakeTestCaseName(testCase.Name);
            var testCaseDto = new TestCaseDto()
            {
                Title = testCaseName,
                Description = testCase.Name,
                TestLevelId = 1,
                TestLevel = new TestLevelDto()
                {
                    Id = 1,
                    Name = "Name",
                    Description = "Description",
                },
            };
            return testCaseDto;
        }

        public IEnumerable<TestResultDto> ToTestResults(IEnumerable<TestSuites> testSuitesCollection)
        {
            var testResultsCollection = new List<TestResultDto>();
            foreach (var testSuitesItem in testSuitesCollection)
            {
                IEnumerable<TestResultDto> testResults = ToTestResults(testSuitesItem);
                testResultsCollection.AddRange(testResults);
            }

            return testResultsCollection;
        }

        public IEnumerable<TestResultDto> ToTestResults(TestSuites testsSuites)
        {
            return ToTestResults(testsSuites.TestItems);
        }

        public IEnumerable<TestResultDto> ToTestResults(IEnumerable<TestSuite> testSuiteCollection)
        {
            var testResultsCollection = new List<TestResultDto>();
            foreach (var testSuite in testSuiteCollection)
            {
                var testResults = ToTestResults(testSuite);
                testResultsCollection.AddRange(testResults);
            }

            return testResultsCollection;
        }

        public IEnumerable<TestResultDto> ToTestResults(TestSuite testSuite)
        {
            var builder = new TestCaseBuilder(testSuite.Name);
            foreach (var testCase in testSuite.TestCases)
            {
                yield return ToTestResult(testCase, builder);
            }
        }

        public TestResultDto ToTestResult(TestCase testCase, TestCaseBuilder builder)
        {
            TestResultDto resultDto = new();
            if (testCase.ResultCode == TestResultCode.Success)
            {
                resultDto.Status = TestStatus.Success;
                resultDto.ActualResult = "SUCCESS";
            }
            else
            {
                resultDto.Status = TestStatus.Failure;
                resultDto.ActualResult = "FAIL";
            }
            TestCaseDto testCaseDto = ToTestCase(testCase, builder);
            resultDto.TestCase = testCaseDto;
            resultDto.ExecutedAt = DateTime.SpecifyKind(testCase.Timestamp, DateTimeKind.Utc);
            return resultDto;
        }
    }
}
