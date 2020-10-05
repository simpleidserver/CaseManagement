using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public class Operation : ICloneable
    {
        public Operation()
        {
            InputParameters = new List<Parameter>();
            OutputParameters = new List<Parameter>();
        }

        public ICollection<Parameter> InputParameters { get; set; }
        public ICollection<Parameter> OutputParameters { get; set; } 

        public object Clone()
        {
            return new Operation
            {
                InputParameters = InputParameters.Select(_ => (Parameter)_.Clone()).ToList(),
                OutputParameters = OutputParameters.Select(_ => (Parameter)_.Clone()).ToList()
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
