using MediatR;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class UpdateHumanTaskDefCompletionDeadlineCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string DeadLineId { get; set; }
        public UpdateDeadLineInfoCommand DeadLineInfo { get; set; }
    }
}
