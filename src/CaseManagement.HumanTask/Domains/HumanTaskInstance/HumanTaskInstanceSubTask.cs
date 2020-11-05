using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
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
