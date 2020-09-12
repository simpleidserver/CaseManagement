using MediatR;

namespace CaseManagement.CMMN.CasePlanInstance.Commands
{
    public class CloseCommand : IRequest<bool>
    {
        public CloseCommand(string casePlanInstanceId)
        {
            CasePlanInstanceId = casePlanInstanceId;
        }

        public string CasePlanInstanceId { get; set; }
    }
}
