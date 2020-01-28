using System.Collections.Generic;

namespace CaseManagement.CMMN.Persistence.Parameters
{
    public class FindCasePlanificationParameter : BaseFindParameter
    {
        public FindCasePlanificationParameter()
        {
            OrderBy = "create_datetime";
        }

        public string GroupBy { get; set; }
        public string CaseInstanceId { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
