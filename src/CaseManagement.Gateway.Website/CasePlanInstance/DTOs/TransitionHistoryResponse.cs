using System;
using System.Runtime.Serialization;

namespace CaseManagement.Gateway.Website.CasePlanInstance.DTOs
{
    [DataContract]
    public class TransitionHistoryResponse
    {
        [DataMember(Name = "transition")]
        public string Transition { get; set; }
        [DataMember(Name = "datetime")]
        public DateTime CreateDateTime { get; set; }
    }
}
