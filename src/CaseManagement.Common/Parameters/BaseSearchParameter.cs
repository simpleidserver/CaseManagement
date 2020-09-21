namespace CaseManagement.Common.Parameters
{
    public class BaseSearchParameter
    {
        public BaseSearchParameter()
        {
            StartIndex = 0;
            Count = 100;
            Order = FindOrders.ASC;
            OrderBy = "create_datetime";
        }

        public int StartIndex { get; set; }
        public int Count { get; set; }
        public string OrderBy { get; set; }
        public FindOrders Order { get; set; }
    }
}
