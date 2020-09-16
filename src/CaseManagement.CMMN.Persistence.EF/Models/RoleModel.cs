using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Persistence.EF.Models
{
    public class RoleModel
    {
        public long Id { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsCaseOwner { get; set; }
        public string CasePlanInstanceId { get; set; }
        public string CasePlanId { get; set; }
        public string CaseWorkerTaskId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public virtual CasePlanInstanceModel CasePlanInstance { get; set; }
        public virtual CasePlanModel CasePlan { get; set; }
        public virtual CaseWorkerTaskModel CaseWorkerTask { get; set; }
        public virtual ICollection<RoleClaimModel> Claims { get; set; }
    }
}
