using System.Linq;

namespace CaseManagement.BPMN.Domains
{
    public class UserTask : BaseTask
    {
        public override object Clone()
        {
            return new UserTask
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
