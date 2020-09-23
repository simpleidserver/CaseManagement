using System.Linq;

namespace CaseManagement.BPMN.Domains
{
    public class SubProcess : BaseActivity
    {
        public override FlowNodeTypes FlowNode => FlowNodeTypes.SUBPROCESS;

        public override object Clone()
        {
            return new SubProcess
            {
                Id = Id,
                Incoming = Incoming.ToList(),
                Name = Name,
                Outgoing = Outgoing.ToList(),
                StartQuantity = StartQuantity
            };
        }
    }
}
