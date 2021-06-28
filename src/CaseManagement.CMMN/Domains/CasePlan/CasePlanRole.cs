using System;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class CasePlanRole : ICloneable
    {
        public string EltId { get; set; }
        public string Name { get; set; }

        public object Clone()
        {
            return new CasePlanRole
            {
                EltId = EltId,
                Name = Name
            };
        }
    }
}
