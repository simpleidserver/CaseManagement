using System.Collections.Generic;

namespace CaseManagement.CMMN.Persistence.Parameters
{
    public class FindFormParameter : BaseFindParameter
    {
        public IEnumerable<string> Ids { get; set; }
    }
}
