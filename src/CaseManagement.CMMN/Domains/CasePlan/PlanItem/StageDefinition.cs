using System.Collections.Generic;

namespace CaseManagement.CMMN.Domains
{
    public class StageDefinition
    {
        public StageDefinition(string name)
        {
            Name = name;
            Elements = new List<CasePlanElement>();
        }

        public string Name { get; set; }
        public ICollection<CasePlanElement> Elements { get; set; }
    }
}
