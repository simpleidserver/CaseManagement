using CaseManagement.HumanTask.Domains;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.Builders
{
    public class CompositionBuilder
    {
        private readonly ICollection<HumanTaskDefinitionSubTask> _subTasks;

        public CompositionBuilder()
        {
            _subTasks = new List<HumanTaskDefinitionSubTask>();
        }

        public CompositionBuilder AddSubTask(string taskName, ICollection<ToPart> toParts)
        {
            _subTasks.Add(new HumanTaskDefinitionSubTask
            {
                TaskName = taskName,
                ToParts = toParts
            });
            return this;
        }

        public ICollection<HumanTaskDefinitionSubTask> Build()
        {
            return _subTasks;
        }
    }
}
