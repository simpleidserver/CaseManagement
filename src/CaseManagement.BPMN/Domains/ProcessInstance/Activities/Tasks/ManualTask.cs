using System.Linq;

namespace CaseManagement.BPMN.Domains.ProcessInstance.Tasks
{
    public class ManualTask : BaseTask
    {
        public override object Clone()
        {
            return new ManualTask
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
