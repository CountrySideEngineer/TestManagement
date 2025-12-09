using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestManagement.Analyze.APP.Model.Xml
{
    internal class TestResultReader
    {
        public TestResultReader() { }

        public virtual IEnumerable<TestSuites> ReadFromDir(string dirPath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            var filesInDir = dirInfo.GetFiles("*.xml", SearchOption.TopDirectoryOnly);

            var testSuitesCollection = new List<TestSuites>();
            foreach (var fileItem in filesInDir)
            {
                string filePath = fileItem.FullName;
                var testSuitesItem = Read(filePath);
                testSuitesCollection.Add(testSuitesItem);
            }

            return testSuitesCollection;
        }

        public virtual TestSuites Read(string filePath)
        {
            try
            {
                using var reader = new StreamReader(filePath, false);
                var serialize = new XmlSerializer(typeof(TestSuites));
                TestSuites testSuites = (TestSuites)serialize.Deserialize(reader)!;

                return testSuites;
            }
            catch (Exception ex)
            when ((ex is ArgumentException) || (ex is ArgumentNullException))
            {
                throw new ArgumentException();
            }
            catch (Exception ex)
            when ((ex is FileNotFoundException) || (ex is DirectoryNotFoundException))
            {
                throw new FileNotFoundException();
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (IOException)
            {
                throw;
            }


        }
    }
}
