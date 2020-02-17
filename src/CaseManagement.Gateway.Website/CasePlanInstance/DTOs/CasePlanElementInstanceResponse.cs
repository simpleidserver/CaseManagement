using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CaseManagement.Gateway.Website.CasePlanInstance.DTOs
{
    [DataContract]
    public class CasePlanElementInstanceResponse
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "version")]
        public int Version { get; set; }
        [DataMember(Name = "create_datetime")]
        public DateTime CreateDateTime { get; set; }
        [DataMember(Name = "case_plan_element_id")]
        public string CasePlanElementId { get; set; }
        [DataMember(Name = "state")]
        public string State { get; set; }
        [DataMember(Name = "state_histories")]
        public ICollection<StateHistoryResponse> StateHistories { get; set; }
        [DataMember(Name = "transition_histories")]
        public ICollection<TransitionHistoryResponse> TransitionHistories { get; set; }
    }
}
