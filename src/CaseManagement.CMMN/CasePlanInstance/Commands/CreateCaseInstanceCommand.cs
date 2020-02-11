using System.Runtime.Serialization;

namespace CaseManagement.CMMN.CasePlanInstance.Commands
{
    [DataContract]
    public class CreateCaseInstanceCommand
    {
        [DataMember(Name = "case_plan_id")]
        public string CasePlanId { get; set; }
        public string NameIdentifier { get; set; }
    }
}