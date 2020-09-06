using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class CasePlanInstanceRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<KeyValuePair<string, string>> Claims { get; set; }
    }
}
