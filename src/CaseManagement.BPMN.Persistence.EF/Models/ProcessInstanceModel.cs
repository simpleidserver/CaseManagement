using System;
using System.Collections.Generic;

namespace CaseManagement.BPMN.Persistence.EF.Models
{
    public class ProcessInstanceModel
    {
        public string AggregateId { get; set; }
        public int Version { get; set; }
        public string InstanceId { get; set; }
        public string ProcessId { get; set; }
        public string CommonId { get; set; }
        public string ProcessFileId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public virtual ICollection<ItemDefinitionModel> ItemDefs { get; set; }
        public virtual ICollection<BPMNInterfaceModel> Interfaces { get; set; }
        public virtual ICollection<MessageModel> Messages { get; set; }
        public virtual ICollection<FlowNodeModel> ElementDefs { get; set; }
        public virtual ICollection<SequenceFlowModel> SequenceFlows { get; set; }
        public virtual ICollection<FlowNodeInstanceModel> ElementInstances { get; set; }
        public virtual ICollection<ExecutionPathModel> ExecutionPathLst { get; set; }
        public virtual ICollection<StateTransitionTokenModel> StateTransitions { get; set; }
    }
}
