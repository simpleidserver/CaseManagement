using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public class Completion : ICloneable
    {
        public Completion()
        {
            CopyLst = new List<Copy>();
        }

        public long Id { get; set; }
        public string Condition { get; set; }
        public ICollection<Copy> CopyLst { get; set; }

        public object Clone()
        {
            return new Completion
            {
                Id = Id,
                Condition = Condition,
                CopyLst = CopyLst.Select(_ => (Copy)_.Clone()).ToList()
            };
        }
    }

    public class Copy : ICloneable
    {
        public long Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        public object Clone()
        {
            return new Copy
            {
                Id = Id,
                From = From,
                To = To
            };
        }
    }
}
