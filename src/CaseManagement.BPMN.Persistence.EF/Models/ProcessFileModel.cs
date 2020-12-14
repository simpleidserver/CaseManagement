using CaseManagement.BPMN.Domains;
using System;

namespace CaseManagement.BPMN.Persistence.EF.Models
{
    public class ProcessFileModel
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public string FileId { get; set; }
        public int NbInstances { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string Payload { get; set; }
        public ProcessFileStatus Status { get; set; }
    }
}
