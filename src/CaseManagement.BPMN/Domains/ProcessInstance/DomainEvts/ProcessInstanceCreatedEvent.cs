using CaseManagement.Common.Domains;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains
{
    [DebuggerDisplay("Create process instance")]
    [Serializable]
    public class ProcessInstanceCreatedEvent : DomainEvent
    {
        public ProcessInstanceCreatedEvent(string id, string aggregateId, int version, string processFileId, string processFileName, ICollection<BPMNInterface> interfaces, ICollection<Message> messages, ICollection<ItemDefinition> itemDefs, ICollection<SequenceFlow> sequenceFlows, DateTime createDateTime) : base(id, aggregateId, version)
        {
            ProcessFileId = processFileId;
            ProcessFileName = processFileName;
            Interfaces = interfaces;
            Messages = messages;
            ItemDefs = itemDefs;
            SequenceFlows = sequenceFlows;
            CreateDateTime = createDateTime;
        }

        public string ProcessFileId { get; set; }
        public string ProcessFileName { get; set; }
        public ICollection<BPMNInterface> Interfaces { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<ItemDefinition> ItemDefs { get; set; }
        public ICollection<SequenceFlow> SequenceFlows { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
