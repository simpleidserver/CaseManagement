using MediatR;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class DeleteHumanTaskDefEscalationCompletionDeadlineCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string DeadLineId { get; set; }
        public string EscalationId { get; set; }
    }
}
