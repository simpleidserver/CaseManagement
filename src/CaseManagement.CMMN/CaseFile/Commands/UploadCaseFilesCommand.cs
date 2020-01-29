using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CaseManagement.CMMN.CaseFile.Commands
{
    [DataContract]
    public class UploadCaseFile
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "content")]
        public string Content { get; set; }
    }

    [DataContract]
    public class UploadCaseFilesCommand
    {
        public UploadCaseFilesCommand()
        {
            Files = new List<UploadCaseFile>();
        }

        public string NameIdentifier { get; set; }
        [DataMember(Name = "files")]
        public IEnumerable<UploadCaseFile> Files { get; set; }
    }
}
