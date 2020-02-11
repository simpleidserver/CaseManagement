using System.Collections.Generic;

namespace CaseManagement.Gateway.Website.Performance.Queries
{
    public class SearchPerformanceQuery
    {
        public IEnumerable<KeyValuePair<string, string>> Queries { get; set; }
    }
}
