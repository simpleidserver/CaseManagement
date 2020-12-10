using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.EF.Models;

namespace CaseManagement.CMMN.Persistence.EF.DomainMapping
{
    public static class CaseFileMapper
    {
        #region To domain

        public static CaseFileAggregate ToDomain(this CaseFileModel caseFile)
        {
            return new CaseFileAggregate
            {
                AggregateId = caseFile.Id,
                Version = caseFile.Version,
                FileId = caseFile.FileId,
                Name = caseFile.Name,
                Description = caseFile.Description,
                CreateDateTime = caseFile.CreateDateTime,
                UpdateDateTime = caseFile.UpdateDateTime,
                Payload = caseFile.SerializedContent,
                Status = (CaseFileStatus)caseFile.Status
            };
        }

        #endregion

        #region To model

        public static CaseFileModel ToModel(this CaseFileAggregate caseFile)
        {
            return new CaseFileModel
            {
                Id = caseFile.AggregateId,
                Version = caseFile.Version,
                FileId = caseFile.FileId,
                Name = caseFile.Name,
                Description = caseFile.Description,
                CreateDateTime = caseFile.CreateDateTime,
                UpdateDateTime = caseFile.UpdateDateTime,
                SerializedContent = caseFile.Payload,
                Status = (int)caseFile.Status
            };
        }

        #endregion
    }
}
