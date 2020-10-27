using CaseManagement.Common.Parameters;
using CaseManagement.HumanTask.Domains;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.Persistence.Parameters
{
    public class SearchHumanTaskInstanceParameter : BaseSearchParameter
    {
        public SearchHumanTaskInstanceParameter()
        {
            StatusLst = new List<HumanTaskInstanceStatus>();
        }

        public string UserIdentifier { get; set; }
        public ICollection<string> GroupNames { get; set; }
        public ICollection<HumanTaskInstanceStatus> StatusLst { get; set; }
        public string ActualOwner { get; set; }
    }
}
