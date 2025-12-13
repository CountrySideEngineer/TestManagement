using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManagement.Analyze.APP.Model
{
    /// <summary>
    /// Represent "ApiSetting" section in "appsettings.json" file.
    /// </summary>
    internal class ApiSetting
    {
        /// <summary>
        /// API base url.
        /// </summary>
        public string BaseUrl { get; set; } = string.Empty;
    }
}
