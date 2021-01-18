using MediatR;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class AddHumanTaskDefEscalationDeadlineCommand : IRequest<string>
    {
        public string Id { get; set; }
        public string DeadlineId { get; set; }
        public string Condition { get; set; }
    }
}
