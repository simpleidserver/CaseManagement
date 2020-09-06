using CaseManagement.CMMN.Domains.CasePlan;
using CaseManagement.CMMN.Infrastructures;
using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Domains.Events
{
    [Serializable]
    public class CasePlanAddedEvent : DomainEvent
    {
        public CasePlanAddedEvent(string id, string aggregateId, int version, string casePlanId, string name, string description, string caseOwner, string caseFileId, DateTime createDateTime, string xmlContent, IEnumerable<CasePlanRole> roles) : base(id, aggregateId, version)
        {
            CasePlanId = casePlanId;
            Name = name;
            Description = description;
            CaseOwner = caseOwner;
            CaseFileId = caseFileId;
            CreateDateTime = createDateTime;
            XmlContent = xmlContent;
            Roles = roles;
        }

        public string CasePlanId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CaseOwner { get; set; }
        public string CaseFileId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string XmlContent { get; set; }
        public IEnumerable<CasePlanRole> Roles { get; set; }
    }
}