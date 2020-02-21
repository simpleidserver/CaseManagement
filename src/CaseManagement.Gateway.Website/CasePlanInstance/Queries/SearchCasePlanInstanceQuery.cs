using System.Collections.Generic;

namespace CaseManagement.Gateway.Website.CasePlanInstance.Queries
{
    public class SearchCasePlanInstanceQuery
    {
        public IEnumerable<KeyValuePair<string, string>> Queries { get; set; }
    }
}
