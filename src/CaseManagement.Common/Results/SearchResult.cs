using System.Collections.Generic;

namespace CaseManagement.Common.Results
{
    public class SearchResult<T>
    {
        public int StartIndex { get; set; }
        public int TotalLength { get; set; }
        public int Count { get; set; }
        public IEnumerable<T> Content { get; set; }
    }
}
