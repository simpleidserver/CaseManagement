using System.Runtime.Serialization;

namespace CaseManagement.Gateway.Website.CaseFile.Commands
{
    [DataContract]
    public class PublishCaseFileCommand
    {
        public string CaseFileId { get; set; }
        [DataMember(Name = "performer")]
        public string Performer { get; set; }
    }
}
