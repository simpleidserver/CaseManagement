using MediatR;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class DeleteHumanTaskDefToPartCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string DeadlineId { get; set; }
        public string EscalationId { get; set; }
        public string ToPartName { get; set; }
    }
}
