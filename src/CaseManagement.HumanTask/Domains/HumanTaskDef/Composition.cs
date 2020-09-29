using System;

namespace CaseManagement.HumanTask.Domains
{
    public class Composition : ICloneable
    {
        public Composition()
        {
            Type = CompositionTypes.SEQUENTIAL;
            InstantiationPattern = InstantiationPatterns.MANUAL;
        }

        /// <summary>
        /// This optional attribute specifies the order in which enclosed sub-tasks are executed.
        /// </summary>
        public CompositionTypes Type { get; set; }
        /// <summary>
        /// This optional attribute specifies the way sub-tasks are instantiated.
        /// </summary>
        public InstantiationPatterns InstantiationPattern { get; set; }
        public string TaskName { get; set; }

        public object Clone()
        {
            return new Composition
            {
                Type = Type,
                InstantiationPattern = InstantiationPattern,
                TaskName = TaskName
            };
        }
    }
}
