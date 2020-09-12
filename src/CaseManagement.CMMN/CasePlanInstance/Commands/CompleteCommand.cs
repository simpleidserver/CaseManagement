using MediatR;

namespace CaseManagement.CMMN.CasePlanInstance.Commands
{
    public class CompleteCommand : IRequest<bool>
    {
        public CompleteCommand(string caseInstanceId, string caseInstanceElementId)
        {
            CaseInstanceId = caseInstanceId;
            CaseInstanceElementId = caseInstanceElementId;
        }

        public string CaseInstanceId { get; set; }
        public string CaseInstanceElementId { get; set; }
    }
}
