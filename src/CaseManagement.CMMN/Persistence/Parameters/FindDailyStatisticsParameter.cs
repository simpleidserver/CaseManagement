using System;

namespace CaseManagement.CMMN.Persistence.Parameters
{
    public class FindDailyStatisticsParameter : BaseFindParameter
    {
        public FindDailyStatisticsParameter() : base()
        {
            OrderBy = "datetime";
        }

        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
    }
}
