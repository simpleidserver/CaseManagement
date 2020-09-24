using CaseManagement.Common.Domains;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains
{
    [DebuggerDisplay("Create process instance")]
    public class ProcessInstanceCreatedEvent : DomainEvent
    {
        public ProcessInstanceCreatedEvent(string id, string aggregateId, int version, string processFileId, string processId, ICollection<BPMNInterface> interfaces, ICollection<Message> messages, ICollection<ItemDefinition> itemDefs, DateTime createDateTime) : base(id, aggregateId, version)
        {
            ProcessFileId = processFileId;
            ProcessId = processId;
            Interfaces = interfaces;
            Messages = messages;
            ItemDefs = itemDefs;
            CreateDateTime = createDateTime;
        }

        public string ProcessFileId { get; set; }
        public string ProcessId { get; set; }
        public ICollection<BPMNInterface> Interfaces { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<ItemDefinition> ItemDefs { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
