using System;

namespace CaseManagement.CMMN.CasePlan.Exceptions
{
    public class UnauthorizedCasePlanException : Exception
    {
        public UnauthorizedCasePlanException(string nameIdentifier, string casePlanId)
        {
            NameIdentifier = nameIdentifier;
            CasePlanId = casePlanId;
        }

        public string NameIdentifier { get; set; }
        public string CasePlanId { get; set; }
    }
}
