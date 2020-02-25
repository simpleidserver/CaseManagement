using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public class StageDefinition : ICloneable
    {
        public StageDefinition(string name)
        {
            Name = name;
            Elements = new List<CasePlanElement>();
        }

        public string Name { get; set; }
        public ICollection<CasePlanElement> Elements { get; set; }

        public object Clone()
        {
            return new StageDefinition(Name)
            {
                Elements = Elements.Select(e => (CasePlanElement)e.Clone()).ToList()
            };
        }
    }
}
