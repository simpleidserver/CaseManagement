using System.Linq;

namespace CaseManagement.BPMN.Domains
{
    public class SubProcess : BaseActivity
    {
        public override object Clone()
        {
            return new SubProcess
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
