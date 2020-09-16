using System;

namespace CaseManagement.CMMN.Persistence.EF.Models
{
    public class CasePlanInstanceWorkerTaskModel
    {
        public long Id { get; set; }
        public string CasePlanInstanceId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string CasePlanElementInstanceId { get; set; }
        public virtual CasePlanInstanceModel CasePlanInstance { get; set; }
    }
}