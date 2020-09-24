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
            Incoming = new List<BaseToken>();
            IsActive = true;
        }

        public string Id { get; set; }
        public string ExecutionPathId { get; set; }
        public string InstanceFlowNodeId { get; set; }
        public string FlowNodeId { get; set; }
        public bool IsActive { get; set; }
        public ICollection<BaseToken> Incoming { get; set; }
        public ICollection<BaseToken> Outgoing { get; set; }

        public object Clone()
        {
            return new ExecutionPointer
            {
                Id = Id,
                ExecutionPathId = ExecutionPathId,
                FlowNodeId = FlowNodeId,
                InstanceFlowNodeId = InstanceFlowNodeId,
                IsActive = IsActive,
                Incoming = Incoming.Select(_ => (BaseToken)_.Clone()).ToList(),
                Outgoing = Incoming.Select(_ => (BaseToken)_.Clone()).ToList()
            };
        }
    }
}
