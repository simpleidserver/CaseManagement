using System.Runtime.Serialization;

namespace CaseManagement.Gateway.Website.CaseFile.Commands
{
    [DataContract]
    public class UpdateCaseFileCommand
    {
        public string CaseFileId { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "payload")]
        public string Payload { get; set; }
        [DataMember(Name = "performer")]
        public string Performer { get; set; }
    }
}