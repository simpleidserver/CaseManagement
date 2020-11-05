using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public class HumanTaskDefinitionSubTask : ICloneable
    {
        public HumanTaskDefinitionSubTask()
        {
            ToParts = new List<ToPart>();
        }

        public long Id { get; set; }
        public string TaskName { get; set; }
        public ICollection<ToPart> ToParts { get; set; }

        public object Clone()
        {
            return new HumanTaskDefinitionSubTask
            {
                Id = Id,
                TaskName = TaskName,
                ToParts = ToParts.Select(_ => (ToPart)_.Clone()).ToList()
            };
        }
    }
}
