using System.Collections.Generic;

namespace CaseManagement.Gateway.Website.CasePlanInstance.Queries
{
    public class SearchAssignedCasePlanInstanceQuery
    {
        public IEnumerable<KeyValuePair<string, string>> Queries { get; set; }
        public string IdentityToken { get; set; }
    }
}
