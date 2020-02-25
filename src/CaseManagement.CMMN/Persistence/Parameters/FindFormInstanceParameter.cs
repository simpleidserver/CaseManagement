using System.Collections.Generic;

namespace CaseManagement.CMMN.Persistence.Parameters
{
    public class FindFormInstanceParameter : BaseFindParameter
    {
        public FindFormInstanceParameter() : base()
        {

        }

        public string CasePlanId { get; set; }
        public string CasePlanInstanceId { get; set; }
        public IEnumerable<string> RoleIds { get; set; }
    }
}
