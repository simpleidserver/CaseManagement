using MediatR;

namespace CaseManagement.HumanTask.HumanTaskInstance.Commands
{
    public class ForceStartHumanTaskInstanceCommand : IRequest<bool>
    {
        public string HumanTaskInstanceId { get; set; }
        public string UserId { get; set; }
    }
}
