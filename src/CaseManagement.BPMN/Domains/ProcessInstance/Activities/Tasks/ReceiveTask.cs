using System.Linq;

namespace CaseManagement.BPMN.Domains
{
    public class ReceiveTask : BaseTask
    {
        public override object Clone()
        {
            return new ReceiveTask
            {
                Id = Id,
                Incoming = Incoming.ToList(),
                Name = Name,
                Outgoing = Outgoing.ToList(),
                StartQuantity = StartQuantity,
                State = State
            };
        }
    }
}
