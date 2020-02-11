using CaseManagement.CMMN.Infrastructures;
using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Domains.Events
{
    public class CasePlanAddedEvent : DomainEvent
    {
        public CasePlanAddedEvent(string id, string aggregateId, int version, string casePlanId, string name, string description, string caseOwner, string caseFileId, DateTime createDateTime, ICollection<Criteria> exitCriterias, ICollection<CasePlanElement> elements) : base(id, aggregateId, version)
        {
            CasePlanId = casePlanId;
            Name = name;
            Description = description;
            CaseOwner = caseOwner;
            CaseFileId = caseFileId;
            CreateDateTime = createDateTime;
            ExitCriterias = exitCriterias;
            Elements = elements;
        }

        public string CasePlanId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CaseOwner { get; set; }
        public string CaseFileId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public ICollection<Criteria> ExitCriterias { get; set; }
        public ICollection<CasePlanElement> Elements { get; set; }
    }
}