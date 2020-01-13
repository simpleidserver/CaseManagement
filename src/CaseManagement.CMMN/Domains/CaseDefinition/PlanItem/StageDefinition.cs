using System.Collections.Generic;

namespace CaseManagement.CMMN.Domains
{
    public class StageDefinition
    {
        public StageDefinition(string name)
        {
            Elements = new List<CaseElementDefinition>();
        }

        public string Name { get; set; }
        public ICollection<CaseElementDefinition> Elements { get; set; }
    }
}
