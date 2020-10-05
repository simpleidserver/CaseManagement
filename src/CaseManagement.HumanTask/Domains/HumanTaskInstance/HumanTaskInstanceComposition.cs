using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public class HumanTaskInstanceComposition : ICloneable
    {
        public HumanTaskInstanceComposition()
        {
            SubTasks = new List<HumanTaskInstanceSubTask>();
        }

        public CompositionTypes Type { get; set; }
        public InstantiationPatterns InstantiationPattern { get; set; }
        public ICollection<HumanTaskInstanceSubTask> SubTasks { get; set; }

        public object Clone()
        {
            return new HumanTaskInstanceComposition
            {
                Type = Type,
                InstantiationPattern = InstantiationPattern,
                SubTasks = SubTasks.Select(_ => (HumanTaskInstanceSubTask)_.Clone()).ToList()
            };
        }
    }

    public class HumanTaskInstanceSubTask : ICloneable
    {
        public HumanTaskInstanceSubTask()
        {
            ToParts = new List<ToPart>();
        }

        public string HumanTaskName { get; set; }
        public ICollection<ToPart> ToParts { get; set; }

        public object Clone()
        {
            return new HumanTaskInstanceSubTask
            {
                HumanTaskName = HumanTaskName,
                ToParts = ToParts.Select(_ => (ToPart)_.Clone()).ToList()
            };
        }
    }
}
