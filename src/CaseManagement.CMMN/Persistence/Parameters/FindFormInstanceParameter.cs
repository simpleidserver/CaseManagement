using System.Collections.Generic;

namespace CaseManagement.CMMN.Persistence.Parameters
{
    public class FindFormInstanceParameter : BaseFindParameter
    {
        public FindFormInstanceParameter() : base()
        {

        }

        public string CasePlanId { get; set; }
        public IEnumerable<string> RoleIds { get; set; }
    }
}
