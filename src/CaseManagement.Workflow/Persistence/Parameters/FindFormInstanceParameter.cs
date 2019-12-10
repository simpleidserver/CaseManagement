using System.Collections.Generic;

namespace CaseManagement.Workflow.Persistence.Parameters
{
    public class FindFormInstanceParameter : BaseFindParameter
    {
        public FindFormInstanceParameter() : base()
        {

        }

        public IEnumerable<string> RoleIds { get; set; }
    }
}
