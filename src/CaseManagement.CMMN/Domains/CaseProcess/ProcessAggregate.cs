using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public class ProcessAggregate : ICloneable
    {
        public ProcessAggregate(string implementationType)
        {
            ImplementationType = implementationType;
            Inputs = new List<string>();
            Outputs = new List<string>();
        }

        public string Id { get; set; }
        public string ImplementationType { get; set; }
        public ICollection<string> Inputs { get; set; }
        public ICollection<string> Outputs { get; set; }

        public virtual object Clone()
        {
            return new ProcessAggregate(ImplementationType)
            {
                Id = Id,
                ImplementationType = ImplementationType,
                Inputs = Inputs.ToList(),
                Outputs = Outputs.ToList()
            };
        }
    }
}
