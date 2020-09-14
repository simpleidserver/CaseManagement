using System;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class CasePlanInstanceWorkerTask : ICloneable
    {
        #region Properties

        public string CasePlanElementInstanceId { get; set; }
        public DateTime CreateDateTime { get; set; }

        #endregion

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
