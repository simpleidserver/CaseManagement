using System;

namespace CaseManagement.CMMN.Domains
{
    public class CaseElementInstancePlanification : ICloneable
    {
        public CaseElementInstancePlanification(string caseElementDefinitionId, DateTime createDateTime)
        {
            CaseElementDefinitionId = caseElementDefinitionId;
            CreateDateTime = createDateTime;
        }

        public string User { get; set; }
        public string CaseElementDefinitionId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? ConfirmationDateTime { get; set; }
        public bool IsConfirmed { get; set; }

        public object Clone()
        {
            return new CaseElementInstancePlanification(CaseElementDefinitionId, CreateDateTime)
            {
                User = User,
                ConfirmationDateTime = ConfirmationDateTime,
                IsConfirmed = IsConfirmed
            };
        }
    }
}
