using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains
{
    [DebuggerDisplay("Case file item added")]
    [Serializable]
    public class CaseFileItemAddedEvent : DomainEvent
    {
        public CaseFileItemAddedEvent(string id, string aggregateId, int version, string casePlanInstanceElementId, string type, string externalValue) : base(id, aggregateId, version)
        {
            CasePlanInstanceElementId = casePlanInstanceElementId;
            Type = type;
            ExternalValue = externalValue;
        }

        public string CasePlanInstanceElementId { get; set; }
        public string Type { get; set; }
        public string ExternalValue { get; set; }
    }
}
