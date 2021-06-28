using System;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class CasePlanInstanceRole : ICloneable
    {
        #region Properties

        public string EltId { get; set; }
        public string Name { get; set; }

        #endregion

        public object Clone()
        {
            return new CasePlanInstanceRole
            {
                EltId = EltId,
                Name = Name
            };
        }
    }
}
