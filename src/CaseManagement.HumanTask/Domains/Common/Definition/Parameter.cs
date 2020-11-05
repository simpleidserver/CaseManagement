using System;

namespace CaseManagement.HumanTask.Domains
{
    public enum ParameterUsages
    {
        INPUT = 0,
        OUTPUT = 1
    }

    public class Parameter : ICloneable
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ParameterUsages Usage { get; set; }
        public ParameterTypes Type { get; set; }
        public bool IsRequired { get; set; }

        public object Clone()
        {
            return new Parameter
            {
                Id = Id,
                Name = Name,
                Type = Type,
                Usage = Usage,
                IsRequired = IsRequired
            };
        }
    }
}
