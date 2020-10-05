using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public class HumanTaskDefinitionCompletionBehavior : ICloneable
    {
        public HumanTaskDefinitionCompletionBehavior()
        {
            Completions = new List<HumanTaskDefinitionCompletion>();
        }

        public CompletionBehaviors CompletionAction { get; set; }
        public ICollection<HumanTaskDefinitionCompletion> Completions { get; set; }

        public object Clone()
        {
            return new HumanTaskDefinitionCompletionBehavior
            {
                CompletionAction = CompletionAction,
                Completions = Completions.Select(_ => (HumanTaskDefinitionCompletion)_.Clone()).ToList()
            };
        }
    }

    public class HumanTaskDefinitionCompletion : ICloneable
    {
        public HumanTaskDefinitionCompletion()
        {
            CopyLst = new List<HumanTaskDefinitionResultCopy>();
        }

        public string Condition { get; set; }
        public ICollection<HumanTaskDefinitionResultCopy> CopyLst { get; set; }

        public object Clone()
        {
            return new HumanTaskDefinitionCompletion
            {
                Condition = Condition,
                CopyLst = CopyLst.Select(_ => (HumanTaskDefinitionResultCopy)_.Clone()).ToList()
            };
        }
    }

    public class HumanTaskDefinitionResultCopy : ICloneable
    {
        public string From { get; set; }
        public string To { get; set; }

        public object Clone()
        {
            return new HumanTaskDefinitionResultCopy
            {
                From = From,
                To = To
            };
        }
    }
}
