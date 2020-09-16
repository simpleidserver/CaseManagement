using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Persistence.EF.Models
{
    public class CaseWorkerTaskModel
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public string CasePlanInstanceId { get; set; }
        public string CasePlanInstanceElementId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public virtual ICollection<RoleModel> Roles { get; set; }
    }
}
