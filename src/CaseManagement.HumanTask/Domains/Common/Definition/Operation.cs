using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public class Operation : ICloneable
    {
        public Operation()
        {
            Parameters = new List<Parameter>();
        }

        public ICollection<Parameter> Parameters { get; set; }

        public object Clone()
        {
            return new Operation
            {
                Parameters = Parameters.Select(_ => (Parameter)_.Clone()).ToList()
            };
        }
    }

    public class Parameter : ICloneable
    {
        public string Name { get; set; }
        public ParameterTypes Type { get; set; }
        public bool IsRequired { get; set; }

        public object Clone()
        {
            return new Parameter
            {
                Name = Name,
                Type = Type,
                IsRequired = IsRequired
            };
        }
    }
}
