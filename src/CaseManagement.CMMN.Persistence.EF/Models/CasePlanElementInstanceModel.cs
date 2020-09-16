using System.Collections.Generic;

namespace CaseManagement.CMMN.Persistence.EF.Models
{
    public class CasePlanElementInstanceModel
    {
        public long Id { get; set; }
        public string EltId { get; set; }
        public long? ParentId { get; set; }
        public string CasePlanInstanceId { get; set; }
        public int Type { get; set; }
        public string SerializedContent { get; set; }
        public virtual CasePlanInstanceModel CasePlanInstance { get; set; }
        public virtual CasePlanElementInstanceModel Parent { get; set; }
        public virtual ICollection<CasePlanElementInstanceModel> Children { get; set; }
    }
}