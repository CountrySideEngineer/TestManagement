using System.Xml.Serialization;

namespace TestManagement.API.Models.Report.Xml
{
    [XmlRoot("testsuites")]
    public class TestSuitesXml
    {
        protected string _path = string.Empty;

        [XmlAttribute("tests")]
        public int Tests { get; set; } = 0;

        [XmlAttribute("failures")]
        public int Failures { get; set; } = 0;

        [XmlAttribute("disabled")]
        public int Disabled { get; set; } = 0;

        [XmlAttribute("errors")]
        public int Errors { get; set; } = 0;

        [XmlAttribute("time")]
        public float Time { get; set; } = 0;

        [XmlAttribute("timestamp")]
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("testsuite")]
        public List<TestSuiteXml> TestItems { get; set; } = new List<TestSuiteXml>();

        /// <summary>
        /// Test name.
        /// </summary>
        public string TestName { get; set; } = string.Empty;
    }

    [XmlRoot("testsuite")]
    public class TestSuiteXml
    {
        public string _path = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("tests")]
        public int Tests { get; set; }

        [XmlAttribute("failures")]
        public int Failures { get; set; }

        [XmlAttribute("disabled")]
        public int Disabled { get; set; }

        [XmlAttribute("errors")]
        public int Errors { get; set; }

        [XmlAttribute("time")]
        public float Time { get; set; }

        [XmlAttribute("timestamp")]
        public DateTime TimeStamp { get; set; }

        [XmlElement("testcase")]
        public List<TestCaseXml> TestCases { get; set; } = new List<TestCaseXml>();
    }

    [XmlRoot("testcase")]
    public class TestCaseXml
    {
        public TestCaseXml()
        {
            Failure = null;
        }

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("status")]
        public string Status { get; set; } = string.Empty;

        [XmlAttribute("result")]
        public string Result { get; set; } = string.Empty;

        [XmlAttribute("time")]
        public float Time { get; set; } = 0;

        [XmlAttribute("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [XmlAttribute("classname")]
        public string ClassName { get; set; } = string.Empty;

        [XmlElement("failure")]
        public Failure? Failure { get; set; }

        public string Judge
        {
            get
            {
                if (null == Failure)
                {
                    return "OK";
                }
                else
                {
                    return "NG";
                }

            }
        }

        public bool IsFail
        {
            get
            {
                if (null == Failure)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }

    [XmlRoot("failure")]
    public class Failure
    {
        [XmlAttribute("message")]
        public string Message { get; set; } = string.Empty;
    }
}
