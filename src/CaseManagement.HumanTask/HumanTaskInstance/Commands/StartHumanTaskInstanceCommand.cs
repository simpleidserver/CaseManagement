using MediatR;

namespace CaseManagement.HumanTask.HumanTaskInstance.Commands
{
    public class StartHumanTaskInstanceCommand : IRequest<bool>
    {
        public string HumanTaskInstanceId { get; set; }
    }
}
