using System.Collections.Generic;

namespace CaseManagement.Gateway.Website.CaseFile.Queries
{
    public class SearchCaseFileHistoryQuery
    {
        public IEnumerable<KeyValuePair<string, string>> Queries { get; set; }
        public string NameIdentifier { get; set; }
        public string CaseFileId { get; set; }
    }
}
