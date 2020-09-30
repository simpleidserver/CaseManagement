using CaseManagement.Common.Parameters;

namespace CaseManagement.HumanTask.Persistence.Parameters
{
    public class FindHumanTaskInstanceHistoryParameter : BaseSearchParameter
    {
        public FindHumanTaskInstanceHistoryParameter()
        {
        }

        public string HumanTaskInstanceId { get; set; }
    }
}
