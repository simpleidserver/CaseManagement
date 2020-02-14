using System.Runtime.Serialization;

namespace CaseManagement.CMMN.CaseFile.Commands
{
    [DataContract]
    public class PublishCaseFileCommand
    {
        public string Id { get; set; }
        [DataMember(Name = "performer")]
        public string Performer { get; set; }
    }
}
