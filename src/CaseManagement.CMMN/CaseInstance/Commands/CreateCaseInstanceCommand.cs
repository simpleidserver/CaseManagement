using System.Runtime.Serialization;

namespace CaseManagement.CMMN.CaseInstance.Commands
{
    [DataContract]
    public class CreateCaseInstanceCommand
    {
        [DataMember(Name = "case_definition_id")]
        public string CaseDefinitionId { get; set; }
        [DataMember(Name = "case_id")]
        public string CasesId { get; set; }
    }
}