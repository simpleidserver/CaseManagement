using MediatR;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class DeleteHumanTaskDefStartDeadlineCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string DeadLineId { get; set; }
    }
}