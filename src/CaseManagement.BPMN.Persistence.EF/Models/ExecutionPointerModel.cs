using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.BPMN.Persistence.EF.Models
{
    public class ExecutionPointerModel
    {
        public string Id { get; set; }
        public string ExecutionPathId { get; set; }
        public string InstanceFlowNodeId { get; set; }
        public string FlowNodeId { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<MessageTokenModel> Tokens { get; set; }
        public ICollection<MessageTokenModel> Incoming => Tokens.Where(_ => _.Direction == MessageTokenDirections.INCOMING).ToList();
        public ICollection<MessageTokenModel> Outgoing => Tokens.Where(_ => _.Direction == MessageTokenDirections.OUTGOING).ToList();
    }
}