using MediatR;
using static CaseManagement.HumanTask.HumanTaskDef.Results.HumanTaskDefResult;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class UpdateHumanTaskDefEscalationStartDeadlineCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string DeadLineId { get; set; }
        public string EscalationId { get; set; }
        public EscalationResult Escalation { get; set; }
    }
}
