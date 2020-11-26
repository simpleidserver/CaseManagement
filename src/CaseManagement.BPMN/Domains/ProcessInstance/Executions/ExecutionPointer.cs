using CaseManagement.BPMN.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.BPMN.Domains
{
    public class ExecutionPointer : ICloneable
    {
        public ExecutionPointer()
        {
            Incoming = new List<MessageToken>();
            Outgoing = new List<MessageToken>();
            IsActive = true;
        }

        public string Id { get; set; }
        public string ExecutionPathId { get; set; }
        public string InstanceFlowNodeId { get; set; }
        public string FlowNodeId { get; set; }
        public bool IsActive { get; set; }
        public ICollection<MessageToken> Incoming { get; set; }
        public ICollection<MessageToken> Outgoing { get; set; }

        public object Clone()
        {
            return new ExecutionPointer
            {
                Id = Id,
                ExecutionPathId = ExecutionPathId,
                FlowNodeId = FlowNodeId,
                InstanceFlowNodeId = InstanceFlowNodeId,
                IsActive = IsActive,
                Incoming = Incoming.Select(_ => (MessageToken)_.Clone()).ToList(),
                Outgoing = Outgoing.Select(_ => (MessageToken)_.Clone()).ToList()
            };
        }
    }
}
