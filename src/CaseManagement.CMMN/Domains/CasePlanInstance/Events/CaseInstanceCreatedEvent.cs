using CaseManagement.Common.Domains;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    [DebuggerDisplay("case plan instance is created")]
    public class CasePlanInstanceCreatedEvent : DomainEvent
    {
        public CasePlanInstanceCreatedEvent(string id, string aggregateId, int version, string nameIdentifier, ICollection<CasePlanInstanceRole> roles, CaseEltInstance stage, DateTime createDateTime, string caseFileId, string casePlanId, string casePlanName, Dictionary<string, string> parameters, ICollection<CaseEltInstance> files) : base(id, aggregateId, version)
        {
            NameIdentifier = nameIdentifier;
            Roles = roles;
            Stage = stage;
            CreateDateTime = createDateTime;
            CaseFileId = caseFileId;
            CasePlanId = casePlanId;
            CasePlanName = casePlanName;
            Parameters = parameters;
            Files = files;
        }

        public string NameIdentifier { get; set; }
        public ICollection<CasePlanInstanceRole> Roles { get; set; }
        public CaseEltInstance Stage { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string CaseFileId { get; set; }
        public string CasePlanId { get; set; }
        public string CasePlanName { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public ICollection<CaseEltInstance> Files { get; set; }
    }
}