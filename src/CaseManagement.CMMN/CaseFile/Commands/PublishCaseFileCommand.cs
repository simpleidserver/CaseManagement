using System.Runtime.Serialization;

namespace CaseManagement.CMMN.CaseFile.Commands
{
    public class PublishCaseFileCommand
    {
        public string Id { get; set; }
        public string Performer { get; set; }
        public bool BypassUserValidation { get; set; }
    }
}
