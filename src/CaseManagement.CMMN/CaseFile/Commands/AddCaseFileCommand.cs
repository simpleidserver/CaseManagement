using System.Runtime.Serialization;

namespace CaseManagement.CMMN.CaseFile.Commands
{
    [DataContract]
    public class AddCaseFileCommand
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "payload")]
        public string Payload { get; set; }
        public string NameIdentifier { get; set; }
    }
}
