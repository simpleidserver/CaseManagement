using MediatR;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class AddHumanTaskDefEscalationStartDeadlineCommand : IRequest<string>
    {
        public string Id { get; set; }
        public string StartDeadlineId { get; set; }
        public string Condition { get; set; }
    }
}
