using System.Collections.Generic;

namespace CaseManagement.CMMN.Domains
{
    public class CasePlanificationAggregate
    {
        public string CaseInstanceId { get; set; }
        public string CaseName { get; set; }
        public string CaseDescription { get; set; }
        public string CaseElementId { get; set; }
        public string CaseElementName { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
    }
}
