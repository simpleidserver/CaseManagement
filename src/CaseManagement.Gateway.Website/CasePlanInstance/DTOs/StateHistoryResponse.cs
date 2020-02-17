using System;
using System.Runtime.Serialization;

namespace CaseManagement.Gateway.Website.CasePlanInstance.DTOs
{
    [DataContract]
    public class StateHistoryResponse
    {
        [DataMember(Name = "state")]
        public string State { get; set; }
        [DataMember(Name = "datetime")]
        public DateTime UpdateDateTime { get; set; }
    }
}
