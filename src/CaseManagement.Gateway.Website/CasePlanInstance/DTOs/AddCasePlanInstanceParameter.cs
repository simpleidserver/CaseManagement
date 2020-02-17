using System.Runtime.Serialization;

namespace CaseManagement.Gateway.Website.CasePlanInstance.DTOs
{
    [DataContract]
    public class AddCasePlanInstanceParameter
    {
        [DataMember(Name = "case_plan_id")]
        public string CasePlanId { get; set; }
    }
}