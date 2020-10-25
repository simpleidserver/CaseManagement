using MediatR;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class AddHumanTaskDefEscalationCompletionDeadlineCommand : IRequest<string>
    {
        public string Id { get; set; }
        public string CompletionDeadlineId { get; set; }
        public string Condition { get; set; }
    }
}
