using System;

namespace CaseManagement.CMMN.Persistence.EF.Models
{
    public class CaseFileModel
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public string FileId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string SerializedContent { get; set; }
        public int Status { get; set; }
    }
}