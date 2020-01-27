using System.Collections.Generic;

namespace CaseManagement.CMMN.Infrastructures.Bus.ConfirmTableItem
{
    public class ConfirmTableItemMessage
    {
        public ConfirmTableItemMessage(string caseInstanceId, string caseElementDefinitionId, string user)
        {
            CaseInstanceId = caseInstanceId;
            CaseElementDefinitionId = caseElementDefinitionId;
            User = user;
        }

        public string CaseInstanceId { get; set; }
        public string CaseElementDefinitionId { get; set; }
        public string User { get; set; }
    }
}
