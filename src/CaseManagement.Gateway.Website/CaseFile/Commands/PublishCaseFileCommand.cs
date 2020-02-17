using System.Runtime.Serialization;

namespace CaseManagement.Gateway.Website.CaseFile.Commands
{
    public class PublishCaseFileCommand
    {
        public string CaseFileId { get; set; }
        public string IdentityToken { get; set; }
    }
}
