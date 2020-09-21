using CaseManagement.Common.Parameters;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Persistence.Parameters
{
    public class FindCaseWorkerTasksParameter : BaseSearchParameter
    {
        public IEnumerable<KeyValuePair<string, string>> Claims { get; set; }
    }
}
