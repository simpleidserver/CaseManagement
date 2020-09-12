using System;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class CasePlanInstanceFileItem : ICloneable
    {
        public string CasePlanElementInstanceId { get; set; }
        public string CaseFileItemType { get; set; }
        public string ExternalValue { get; set; }

        public object Clone()
        {
            return new CasePlanInstanceFileItem
            {
                CaseFileItemType = CaseFileItemType,
                CasePlanElementInstanceId = CasePlanElementInstanceId,
                ExternalValue = ExternalValue
            };
        }
    }
}
