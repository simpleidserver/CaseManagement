using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CaseManagement.Gateway.Website.FormInstance.DTOs
{
    [DataContract]
    public class FormInstanceResponse
    {
        [DataMember(Name= "id")]
        public string Id { get; set; }
        [DataMember(Name= "form_id")]
        public string FormId { get; set; }
        [DataMember(Name = "case_plan_id")]
        public string CasePlanId { get; set; }
        [DataMember(Name = "case_plan_instance_id")]
        public string CasePlanInstanceId { get; set; }
        [DataMember(Name = "case_plan_element_instance_id")]
        public string CaseElementInstanceId { get; set; }
        [DataMember(Name = "content")]
        public ICollection<FormInstanceElementResponse> Content { get; set; }
        [DataMember(Name = "status")]
        public string Status { get; set; }
        [DataMember(Name = "create_datetime")]
        public DateTime CreateDateTime { get; set; }
        [DataMember(Name = "update_datetime")]
        public DateTime UpdateDateTime { get; set; }
        [DataMember(Name = "performer")]
        public string PerformerRole { get; set; }
    }
}