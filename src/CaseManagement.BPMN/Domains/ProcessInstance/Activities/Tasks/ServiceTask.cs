using System.Linq;

namespace CaseManagement.BPMN.Domains
{
    public class ServiceTask : BaseTask
    {
        public override object Clone()
        {
            return new ServiceTask
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
