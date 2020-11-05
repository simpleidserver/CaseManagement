using CaseManagement.HumanTask.Domains;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.Builders
{
    public class CompletionBehaviorBuilder
    {
        private readonly ICollection<Completion> _completions;

        public CompletionBehaviorBuilder()
        {
            _completions = new List<Completion>();
        }

        public CompletionBehaviorBuilder AddCompletion(string condition, ICollection<Copy> copyLst)
        {
            _completions.Add(new Completion
            {
                Condition = condition,
                CopyLst = copyLst
            });
            return this;
        }

        public ICollection<Completion> Build()
        {
            return _completions;
        }
    }
}
