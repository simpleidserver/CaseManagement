using CaseManagement.Common.Parameters;

namespace CaseManagement.BPMN.Persistence.Parameters
{
    public class FindProcessFilesParameter : BaseSearchParameter
    {
        public FindProcessFilesParameter()
        {
            TakeLatest = false;
            OrderBy = "create_datetime";
            Order = FindOrders.DESC;
            Count = 100;
            StartIndex = 0;
        }

        public bool TakeLatest { get; set; }
        public string FileId { get; set; }
    }
}
