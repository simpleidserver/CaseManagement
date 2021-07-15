using MediatR;

namespace CaseManagement.HumanTask.HumanTaskInstance.Commands
{
    public class ForceClaimHumanTaskInstanceCommand : IRequest<bool>
    {
        public string HumanTaskInstanceId { get; set; }
        public string UserId { get; set; }
    }
}
