using System;
using System.Runtime.Serialization;

namespace CaseManagement.Gateway.Website.CaseWorkerTask.DTOs
{
    [DataContract]
    public class CaseWorkerTaskResponse
    {
        [DataMember(Name = "performer")]
        public string PerformerRole { get; set; }
        [DataMember(Name = "case_plan_id")]
        public string CasePlanId { get; set; }
        [DataMember(Name = "case_plan_instance_id")]
        public string CasePlanInstanceId { get; set; }
        [DataMember(Name = "case_plan_element_instance_id")]
        public string CasePlanElementInstanceId { get; set; }
        [DataMember(Name = "type")]
        public string TaskType { get; set; }
        [DataMember(Name = "status")]
        public string Status { get; set; }
        [DataMember(Name = "create_datetime")]
        public DateTime CreateDateTime { get; set; }
        [DataMember(Name = "update_datetime")]
        public DateTime UpdateDateTime { get; set; }
    }
}
