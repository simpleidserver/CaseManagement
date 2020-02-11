using System;

namespace CaseManagement.CMMN.Persistence.Parameters
{
    public class FindPerformanceParameter : BaseFindParameter
    {
        public FindPerformanceParameter() : base()
        {
            OrderBy = "datetime";
        }

        public string MachineName { get; set; }
        public DateTime? StartDateTime { get; set; }
        public string GroupBy { get; set; }
    }
}
