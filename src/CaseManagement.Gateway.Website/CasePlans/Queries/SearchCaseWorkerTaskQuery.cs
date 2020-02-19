using System;
using System.Collections.Generic;

namespace CaseManagement.Gateway.Website.CasePlans.Queries
{
    public class SearchCaseWorkerTaskQuery
    {
        public IEnumerable<KeyValuePair<string, string>> Queries { get; set; }
        public string CasePlanId { get; set; }
    }
}
