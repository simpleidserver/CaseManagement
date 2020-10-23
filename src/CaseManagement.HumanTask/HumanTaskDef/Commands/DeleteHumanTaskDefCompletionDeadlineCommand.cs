using MediatR;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class DeleteHumanTaskDefCompletionDeadlineCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string DeadLineId { get; set; }
    }
}
