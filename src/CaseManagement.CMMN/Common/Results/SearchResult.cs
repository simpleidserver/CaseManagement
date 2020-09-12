using System.Collections.Generic;

namespace CaseManagement.CMMN.Common
{
    public class SearchResult<T>
    {
        public int StartIndex { get; set; }
        public int TotalLength { get; set; }
        public int Count { get; set; }
        public IEnumerable<T> Content { get; set; }
    }
}
