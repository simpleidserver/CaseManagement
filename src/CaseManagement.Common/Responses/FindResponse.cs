using System.Collections.Generic;

namespace CaseManagement.Common.Responses
{
    public class FindResponse<T>
    {
        public FindResponse()
        {
            Content = new List<T>();
        }

        public int StartIndex { get; set; }
        public int TotalLength { get; set; }
        public int Count { get; set; }
        public ICollection<T> Content { get; set; }
    }
}
