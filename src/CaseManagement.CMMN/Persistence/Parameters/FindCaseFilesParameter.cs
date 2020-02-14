namespace CaseManagement.CMMN.Persistence.Parameters
{
    public class FindCaseFilesParameter : BaseFindParameter
    {
        public FindCaseFilesParameter()
        {
            TakeLatest = false;
            OrderBy = "create_datetime";
            Order = FindOrders.ASC;
            Count = 100;
            StartIndex = 0;
        }

        public string Owner { get; set; }
        public string Text { get; set; }
        public bool TakeLatest { get; set; }
        public string CaseFileId { get; set; }
    }
}