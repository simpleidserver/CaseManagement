using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    [DebuggerDisplay("Set variable '{Key}'='{Value}'")]
    public class VariableUpdatedEvent : DomainEvent
    {
        public VariableUpdatedEvent(string id, string aggregateId, int version, string key, string value, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            Key = key;
            Value = value;
            UpdateDateTime = updateDateTime;
        }

        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
