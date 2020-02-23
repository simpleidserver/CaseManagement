using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CaseManagement.Gateway.Website.CasePlanInstance.DTOs
{
    [DataContract]
    public class CasePlanInstanceResponse
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "create_datetime")]
        public DateTime CreateDateTime { get; set; }
        [DataMember(Name = "case_plan_id")]
        public string CasePlanId { get; set; }
        [DataMember(Name = "context")]
        public JObject Context { get; set; }
        [DataMember(Name = "state")]
        public string State { get; set; }
        [DataMember(Name = "transition_histories")]
        public ICollection<TransitionHistoryResponse> TransitionHistories { get; set; }
        [DataMember(Name = "state_histories")]
        public ICollection<StateHistoryResponse> StateHistories { get; set; }
        [DataMember(Name = "elements")]
        public ICollection<CasePlanElementInstanceResponse> Elements { get; set; }
    }
}
