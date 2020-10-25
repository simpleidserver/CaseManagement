using MediatR;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class DeleteHumanTaskDefEscalationStartDeadlineCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string DeadLineId { get; set; }
        public string EscalationId { get; set; }
    }
}
