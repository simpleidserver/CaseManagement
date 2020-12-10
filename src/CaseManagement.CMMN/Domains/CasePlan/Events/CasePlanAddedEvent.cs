using CaseManagement.Common.Domains;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    [DebuggerDisplay("Case plan added")]
    public class CasePlanAddedEvent : DomainEvent
    {
        public CasePlanAddedEvent(string id, string aggregateId, int version, string casePlanId, string name, string description, string caseFileId, DateTime createDateTime, string xmlContent, ICollection<CasePlanRole> roles, ICollection<CasePlanFileItem> files) : base(id, aggregateId, version)
        {
            CasePlanId = casePlanId;
            Name = name;
            Description = description;
            CaseFileId = caseFileId;
            CreateDateTime = createDateTime;
            XmlContent = xmlContent;
            Roles = roles;
            Files = files;
        }

        public string CasePlanId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CaseFileId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string XmlContent { get; set; }
        public ICollection<CasePlanRole> Roles { get; set; }
        public ICollection<CasePlanFileItem> Files { get; set; }
    }
}
