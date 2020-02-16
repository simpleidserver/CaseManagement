using System.Collections.Generic;

namespace CaseManagement.Gateway.Website.CasePlans.Queries
{
    public class SearchMyLatestCasePlanQuery
    {
        public IEnumerable<KeyValuePair<string, string>> Queries { get; set; }
        public string NameIdentifier { get; set; }
    }
}
