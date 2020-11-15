using MediatR;
using static CaseManagement.HumanTask.HumanTaskDef.Results.HumanTaskDefResult;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class AddCallbackOperationCommand : IRequest<string>
    {
        public string HumanTaskDefId { get; set; }
        public CallbackOperationResult Operation { get; set; }
    }
}
