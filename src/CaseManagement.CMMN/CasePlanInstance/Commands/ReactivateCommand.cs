using MediatR;

namespace CaseManagement.CMMN.CasePlanInstance.Commands
{
    public class ReactivateCommand : IRequest<bool>
    {
        public ReactivateCommand(string caseInstanceId, string caseInstanceElementId)
        {
            CaseInstanceId = caseInstanceId;
            CaseInstanceElementId = caseInstanceElementId;
        }

        public string CaseInstanceId { get; set; }
        public string CaseInstanceElementId { get; set; }
    }
}
