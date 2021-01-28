using MediatR;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class UpdateHumanTaskDefEscalationDeadlineCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string DeadlineId { get; set; }
        public string EscalationId { get; set; }
        public string Condition { get; set; }
        public string NotificationId { get; set; }
    }
}
