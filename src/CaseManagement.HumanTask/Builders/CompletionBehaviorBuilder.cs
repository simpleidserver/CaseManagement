using CaseManagement.HumanTask.Domains;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.Builders
{
    public class CompletionBehaviorBuilder
    {
        private readonly CompletionBehavior _completion;

        public CompletionBehaviorBuilder(CompletionBehaviors behavior)
        {
            _completion = new CompletionBehavior
            {
                CompletionAction = behavior
            };
        }

        public CompletionBehaviorBuilder AddCompletion(string condition, ICollection<Copy> copyLst)
        {
            _completion.Completions.Add(new Completion
            {
                Condition = condition,
                CopyLst = copyLst
            });
            return this;
        }

        public CompletionBehavior Build()
        {
            return _completion;
        }
    }
}
