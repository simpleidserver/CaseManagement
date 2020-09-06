using CaseManagement.CMMN.Infrastructures;
using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Domains.CasePlanInstance.Events
{
    [Serializable]
    public class CasePlanInstanceCreatedEvent : DomainEvent
    {
        public CasePlanInstanceCreatedEvent(string id, string aggregateId, int version, IEnumerable<CasePlanInstanceRole> roles, string xmlContent, DateTime createDateTime) : base(id, aggregateId, version)
        {
            Roles = roles;
            XmlContent = xmlContent;
            CreateDateTime = createDateTime;
        }

        public IEnumerable<CasePlanInstanceRole> Roles { get; set; }
        public string XmlContent { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
