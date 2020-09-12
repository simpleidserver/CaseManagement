using System;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class CasePlanInstanceWorkerTask : ICloneable
    {
        public string CasePlanElementInstanceId { get; set; }
        public DateTime CreateDateTime { get; set; }

        public object Clone()
        {
            return new CasePlanInstanceWorkerTask
            {
                CasePlanElementInstanceId = CasePlanElementInstanceId,
                CreateDateTime = CreateDateTime
            };
        }
    }
}
