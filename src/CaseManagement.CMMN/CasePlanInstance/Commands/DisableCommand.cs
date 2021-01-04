using MediatR;

namespace CaseManagement.CMMN.CasePlanInstance.Commands
{
    public class DisableCommand : IRequest<bool>
    {
        public DisableCommand(string casePlanInstanceId, string casePlanElementInstanceId)
        {
            CasePlanInstanceId = casePlanInstanceId;
            CasePlanElementInstanceId = casePlanElementInstanceId;
        }

        public string CasePlanInstanceId { get; set; }
        public string CasePlanElementInstanceId { get; set; }
    }
}
