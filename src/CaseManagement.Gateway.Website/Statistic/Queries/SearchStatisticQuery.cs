using System.Collections.Generic;

namespace CaseManagement.Gateway.Website.Statistic.Queries
{
    public class SearchStatisticQuery
    {
        public IEnumerable<KeyValuePair<string, string>> Queries { get; set; }
    }
}
