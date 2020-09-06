using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public class StageElementInstance : BaseTaskOrStageElementInstance
    {
        public StageElementInstance()
        {
            Children = new List<CasePlanElementInstance>();
        }

        public ICollection<CasePlanElementInstance> Children { get; set; }
        public int Length => Children.Count();

        public void AddChild(CasePlanElementInstance child)
        {
            Children.Add(child);
        }
    }
}
