using CaseManagement.CMMN.Domains;
using System;

namespace CaseManagement.CMMN.CaseFile.Results
{
    public class CaseFileResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Payload { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public int Version { get; set; }
        public string FileId { get; set; }
        public string Owner { get; set; }
        public string Status { get; set; }

        public static CaseFileResult ToDto(CaseFileAggregate caseFile)
        {
            return new CaseFileResult
            {
                CreateDateTime = caseFile.CreateDateTime,
                Description = caseFile.Description,
                Id = caseFile.AggregateId,
                Name = caseFile.Name,
                FileId = caseFile.FileId,
                Owner = caseFile.Owner,
                Payload = caseFile.Payload,
                Status = Enum.GetName(typeof(CaseFileStatus), caseFile.Status),
                UpdateDateTime = caseFile.UpdateDateTime,
                Version = caseFile.Version
            };
        }
    }
}
