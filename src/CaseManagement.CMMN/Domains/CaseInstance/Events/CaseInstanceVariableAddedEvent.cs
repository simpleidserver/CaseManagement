using CaseManagement.CMMN.Infrastructures;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Set variable {Key}={Value}")]
    public class CaseInstanceVariableAddedEvent : DomainEvent
    {
        public CaseInstanceVariableAddedEvent(string id, string aggregateId, int version, string key, string value) : base(id, aggregateId, version)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }
    }
}
