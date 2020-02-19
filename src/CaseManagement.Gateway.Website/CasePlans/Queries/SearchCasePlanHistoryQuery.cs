using System.Collections.Generic;

namespace CaseManagement.Gateway.Website.CasePlans.Queries
{
    public class SearchCasePlanHistoryQuery
    {
        public IEnumerable<KeyValuePair<string, string>> Queries { get; set; }
        public string NameIdentifier { get; set; }
        public string CasePlanId { get; set; }
    }
}
