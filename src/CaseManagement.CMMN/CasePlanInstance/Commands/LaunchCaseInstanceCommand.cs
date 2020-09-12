using MediatR;

namespace CaseManagement.CMMN.CasePlanInstance.Commands
{
    public class LaunchCaseInstanceCommand : IRequest<bool>
    {
        public string CasePlanInstanceId { get; set; }
    }
}