using System.Collections.Generic;

namespace CaseManagement.Gateway.Website.Role.Queries
{
    public class SearchRoleQuery
    {
        public IEnumerable<KeyValuePair<string, string>> Queries { get; set; }
    }
}
