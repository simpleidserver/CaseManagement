using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Persistence.EF.Models
{
    public class CasePlanInstanceModel
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public string CaseFileId { get; set; }
        public string CasePlanId { get; set; }
        public string NameIdentifier { get; set; }
        public string Name { get; set; }
        public int? CaseState { get; set; }
        public string ExecutionContext { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public virtual ICollection<RoleModel> Roles { get; set; }
        public virtual ICollection<CasePlanInstanceFileItemModel> Files { get; set; }
        public virtual ICollection<CasePlanInstanceWorkerTaskModel> WorkerTasks { get; set; }
        public virtual ICollection<CasePlanElementInstanceModel> Children { get; set; }
        public RoleModel CaseOwner => Roles.FirstOrDefault(_ => _.IsCaseOwner);
    }
}
