using System.Collections.Generic;

namespace CaseManagement.Gateway.Website.CasePlans.Queries
{
    public class SearchFormInstanceQuery
    {
        public string CasePlanId { get; set; }
        public IEnumerable<KeyValuePair<string, string>> Queries { get; set; }
    }
}
