using CaseManagement.BPMN.Domains;
using System;

namespace CaseManagement.BPMN.ProcessFile.Results
{
    public class ProcessFileResult
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public string FileId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string Payload { get; set; }
        public string Status { get; set; }

        public static ProcessFileResult ToDto(ProcessFileAggregate processFile)
        {
            return new ProcessFileResult
            {
                Id = processFile.AggregateId,
                Version = processFile.Version,
                CreateDateTime = processFile.CreateDateTime,
                Description = processFile.Description,
                FileId = processFile.FileId,
                Name = processFile.Name,
                Payload = processFile.Payload,
                Status = Enum.GetName(typeof(ProcessFileStatus), processFile.Status),
                UpdateDateTime = processFile.UpdateDateTime
            };
        }
    }
}
