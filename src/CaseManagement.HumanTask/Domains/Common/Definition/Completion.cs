using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public class CompletionBehavior : ICloneable
    {
        public CompletionBehavior()
        {
            Completions = new List<Completion>();
        }

        public CompletionBehaviors CompletionAction { get; set; }
        public ICollection<Completion> Completions { get; set; }

        public object Clone()
        {
            return new CompletionBehavior
            {
                CompletionAction = CompletionAction,
                Completions = Completions.Select(_ => (Completion)_.Clone()).ToList()
            };
        }
    }

    public class Completion : ICloneable
    {
        public Completion()
        {
            CopyLst = new List<Copy>();
        }

        public string Condition { get; set; }
        public ICollection<Copy> CopyLst { get; set; }

        public object Clone()
        {
            return new Completion
            {
                Condition = Condition,
                CopyLst = CopyLst.Select(_ => (Copy)_.Clone()).ToList()
            };
        }
    }

    public class Copy : ICloneable
    {
        public string From { get; set; }
        public string To { get; set; }

        public object Clone()
        {
            return new Copy
            {
                From = From,
                To = To
            };
        }
    }
}
