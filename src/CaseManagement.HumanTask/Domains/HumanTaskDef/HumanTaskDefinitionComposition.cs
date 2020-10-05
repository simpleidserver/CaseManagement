using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public class HumanTaskDefinitionComposition : ICloneable
    {
        public HumanTaskDefinitionComposition()
        {
            Type = CompositionTypes.SEQUENTIAL;
            InstantiationPattern = InstantiationPatterns.MANUAL;
            SubTasks = new List<HumanTaskDefinitionSubTask>();
        }

        /// <summary>
        /// This optional attribute specifies the order in which enclosed sub-tasks are executed.
        /// </summary>
        public CompositionTypes Type { get; set; }
        /// <summary>
        /// This optional attribute specifies the way sub-tasks are instantiated.
        /// </summary>
        public InstantiationPatterns InstantiationPattern { get; set; }
        public ICollection<HumanTaskDefinitionSubTask> SubTasks { get; set; }

        public object Clone()
        {
            return new HumanTaskDefinitionComposition
            {
                Type = Type,
                InstantiationPattern = InstantiationPattern,
                SubTasks = SubTasks.Select(_ => (HumanTaskDefinitionSubTask)_.Clone()).ToList()
            };
        }
    }

    public class HumanTaskDefinitionSubTask : ICloneable
    {
        public HumanTaskDefinitionSubTask()
        {
            ToParts = new List<ToPart>();
        }

        public string TaskName { get; set; }
        public ICollection<ToPart> ToParts { get; set; }

        public object Clone()
        {
            return new HumanTaskDefinitionSubTask
            {
                TaskName = TaskName,
                ToParts = ToParts.Select(_ => (ToPart)_.Clone()).ToList()
            };
        }
    }
}
