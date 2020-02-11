using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CaseManagement.Gateway.Website
{
    [DataContract]
    public class FindResponse<T>
    {
        public FindResponse()
        {
            Content = new List<T>();
        }

        [DataMember(Name = "start_index")]
        public int StartIndex { get; set; }
        [DataMember(Name = "total_length")]
        public int TotalLength { get; set; }
        [DataMember(Name = "count")]
        public int Count { get; set; }
        [DataMember(Name = "content")]
        public ICollection<T> Content { get; set; }
    }
}
