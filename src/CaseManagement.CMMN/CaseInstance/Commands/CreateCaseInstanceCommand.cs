using System.Runtime.Serialization;

namespace CaseManagement.CMMN.CaseInstance.Commands
{
    [DataContract]
    public class CreateCaseInstanceCommand
    {
        [DataMember(Name = "case_definition_id")]
        public string CaseDefinitionId { get; set; }
        public string NameIdentifier { get; set; }
    }
}