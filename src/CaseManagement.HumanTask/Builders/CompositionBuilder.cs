using CaseManagement.HumanTask.Domains;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.Builders
{
    public class CompositionBuilder
    {
        private readonly CompositionTypes _type;
        private readonly InstantiationPatterns _pattern;
        private readonly HumanTaskDefinitionComposition _composition;

        public CompositionBuilder(CompositionTypes type, InstantiationPatterns pattern)
        {
            _type = type;
            _pattern = pattern;
            _composition = new HumanTaskDefinitionComposition
            {
                InstantiationPattern = pattern,
                Type = type
            };
        }

        public CompositionBuilder AddSubTask(string taskName, ICollection<ToPart> toParts)
        {
            _composition.SubTasks.Add(new HumanTaskDefinitionSubTask
            {
                TaskName = taskName,
                ToParts = toParts
            });
            return this;
        }

        public HumanTaskDefinitionComposition Build()
        {
            return _composition;
        }
    }
}
