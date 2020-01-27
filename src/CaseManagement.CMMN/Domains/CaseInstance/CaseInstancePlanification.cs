using System;

namespace CaseManagement.CMMN.Domains
{
    public class CaseElementInstancePlanification : ICloneable
    {
        public CaseElementInstancePlanification(string user, string caseElementDefinitionId, DateTime createDateTime)
        {
            User = user;
            CaseElementDefinitionId = caseElementDefinitionId;
            CreateDateTime = createDateTime;
        }

        public string User { get; set; }
        public string CaseElementDefinitionId { get; set; }
        public DateTime CreateDateTime { get; set; }

        public object Clone()
        {
            return new CaseElementInstancePlanification(User, CaseElementDefinitionId, CreateDateTime);
        }
    }
}
