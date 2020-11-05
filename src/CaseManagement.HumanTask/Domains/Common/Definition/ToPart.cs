using System;

namespace CaseManagement.HumanTask.Domains
{
    public class ToPart : ICloneable
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Expression { get; set; }

        public object Clone()
        {
            return new ToPart
            {
                Id = Id,
                Name = Name,
                Expression = Expression
            };
        }
    }
}
