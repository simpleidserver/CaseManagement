using System.Linq;

namespace CaseManagement.BPMN.Domains
{
    public class SendTask : BaseTask
    {
        public override object Clone()
        {
            return new SendTask
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
