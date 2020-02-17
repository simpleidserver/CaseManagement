using System.Collections.Generic;

namespace CaseManagement.Gateway.Website.CasePlans.Queries
{
    public class SearchCasePlanInstanceQuery
    {
        public IEnumerable<KeyValuePair<string, string>> Queries { get; set; }
        public string CasePlanId { get; set; }
        public string Owner { get; set; }
    }
}
