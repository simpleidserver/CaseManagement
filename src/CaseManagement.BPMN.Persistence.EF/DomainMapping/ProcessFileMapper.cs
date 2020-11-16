using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Persistence.EF.Models;

namespace CaseManagement.BPMN.Persistence.EF.DomainMapping
{
    public static class ProcessFileMapper
    {
        public static ProcessFileModel ToModel(this ProcessFileAggregate processFile)
        {
            return new ProcessFileModel
            {
                CreateDateTime = processFile.CreateDateTime,
                Description = processFile.Description,
                FileId = processFile.FileId,
                Id = processFile.AggregateId,
                Name = processFile.Name,
                Payload = processFile.Payload,
                Status = processFile.Status,
                UpdateDateTime = processFile.UpdateDateTime,
                Version = processFile.Version
            };
        }

        public static ProcessFileAggregate ToDomain(this ProcessFileModel processFile)
        {
            return new ProcessFileAggregate
            {
                CreateDateTime = processFile.CreateDateTime,
                Description = processFile.Description,
                FileId = processFile.FileId,
                AggregateId = processFile.Id,
                Name = processFile.Name,
                Payload = processFile.Payload,
                Status = processFile.Status,
                UpdateDateTime = processFile.UpdateDateTime,
                Version = processFile.Version
            };
        }
    }
}
