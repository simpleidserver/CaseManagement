using System;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class CasePlanRole : ICloneable
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public object Clone()
        {
            return new CasePlanRole
            {
                Id = Id,
                Name = Name
            };
        }
    }
}
