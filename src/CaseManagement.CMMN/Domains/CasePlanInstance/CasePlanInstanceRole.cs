using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class CasePlanInstanceRole : ICloneable
    {
        public CasePlanInstanceRole()
        {
            Claims = new List<KeyValuePair<string, string>>();
        }

        #region Properties

        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<KeyValuePair<string, string>> Claims { get; set; }

        #endregion

        public object Clone()
        {
            return new CasePlanInstanceRole
            {
                Id = Id,
                Name = Name,
                Claims = Claims.ToList()
            };
        }
    }
}
