using System;

namespace CaseManagement.CMMN.Persistence.Parameters
{
    public class FindPerformanceStatisticsParameter : BaseFindParameter
    {
        public FindPerformanceStatisticsParameter() : base()
        {
            OrderBy = "datetime";
        }

        public string MachineName { get; set; }
        public DateTime? StartDateTime { get; set; }
        public string GroupBy { get; set; }
    }
}
