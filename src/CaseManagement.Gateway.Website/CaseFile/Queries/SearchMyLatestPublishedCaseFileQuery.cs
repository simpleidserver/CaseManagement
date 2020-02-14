using System.Collections.Generic;

namespace CaseManagement.Gateway.Website.CaseFile.Queries
{
    public class SearchMyLatestPublishedCaseFileQuery
    {
        public SearchMyLatestPublishedCaseFileQuery(IEnumerable<KeyValuePair<string, string>> queries, string nameIdentifier)
        {
            Queries = queries;
            NameIdentifier = nameIdentifier;
        }

        public IEnumerable<KeyValuePair<string, string>> Queries { get; set; }
        public string NameIdentifier { get; set; }
    }
}
