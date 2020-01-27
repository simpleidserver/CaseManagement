using System;

namespace CaseManagement.CMMN.CaseInstance.Exceptions
{
    public class UnknownCaseElementDefinitionException : Exception
    {
        public UnknownCaseElementDefinitionException(string caseInstanceId, string caseElementDefinitionId)
        {
            CaseInstanceId = caseInstanceId;
            CaseElementDefinitionId = caseElementDefinitionId;
        }

        public string CaseInstanceId { get; set; }
        public string CaseElementDefinitionId { get; set; }
    }
}
