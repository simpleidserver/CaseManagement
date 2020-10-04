using System;

namespace CaseManagement.HumanTask.Domains
{
    public class ToPart : ICloneable
    {
        public string Name { get; set; }
        public string Expression { get; set; }

        public object Clone()
        {
            return new ToPart
            {
                Name = Name,
                Expression = Expression
            };
        }
    }
}
