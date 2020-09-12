using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    [DebuggerDisplay("case plan instance is created")]
    public class CasePlanInstanceCreatedEvent : DomainEvent
    {
        public CasePlanInstanceCreatedEvent(string id, string aggregateId, int version, string caseOwner, ICollection<CasePlanInstanceRole> roles, ICollection<CasePlanInstanceRole> permissions, string jsonContent, DateTime createDateTime, string casePlanId, Dictionary<string, string> parameters, ICollection<CasePlanFileItem> files) : base(id, aggregateId, version)
        {
            CaseOwner = caseOwner;
            Roles = roles;
            Permissions = permissions;
            JsonContent = jsonContent;
            CreateDateTime = createDateTime;
            CasePlanId = casePlanId;
            Parameters = parameters;
            Files = files;
        }

        public string CaseOwner { get; set; }
        public ICollection<CasePlanInstanceRole> Roles { get; set; }
        public ICollection<CasePlanInstanceRole> Permissions { get; set; }
        public string JsonContent { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string CasePlanId { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public ICollection<CasePlanFileItem> Files { get; set; }
    }
}