namespace CaseManagement.CMMN.Persistence.Parameters
{
    public class BaseFindParameter
    {
        public BaseFindParameter()
        {
            StartIndex = 0;
            Count = 100;
            Order = FindOrders.ASC;
            OrderBy = "create_datetime";
        }

        public int StartIndex { get; set; }
        public int Count { get; set; }
        public FindOrders Order { get; set; }
        public string OrderBy { get; set; }
    }
}
