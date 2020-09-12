using CaseManagement.CMMN.Domains;
using System;

namespace CaseManagement.CMMN.CaseWorkerTask.Results
{
    public class CaseWorkerTaskResult
    {
        public string CasePlanInstanceId { get; set; }
        public string CasePlanInstanceElementId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }

        public static CaseWorkerTaskResult ToDTO(CaseWorkerTaskAggregate cwt)
        {
            return new CaseWorkerTaskResult
            {
                CasePlanInstanceElementId = cwt.CasePlanInstanceElementId,
                CasePlanInstanceId = cwt.CasePlanInstanceId,
                CreateDateTime = cwt.CreateDateTime,
                UpdateDateTime = cwt.UpdateDateTime
            };
        }
    }
}
