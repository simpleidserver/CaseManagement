using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Persistence.EF.Models
{
    public class CasePlanModel
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public string CasePlanId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CaseOwner { get; set; }
        public string CaseFileId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string SerializedContent { get; set; }
        public virtual ICollection<RoleModel> Roles { get; set; }
        public virtual ICollection<CasePlanFileItemModel> Files { get; set; }
    }
}
