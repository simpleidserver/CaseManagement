using System;

namespace CaseManagement.BPMN.Domains
{
    public class Signal : ICloneable
    {
        public string Name { get; set; }
        public string StructuredRef { get; set; }

        public object Clone()
        {
            return new Signal
            {
                Name = Name,
                StructuredRef = StructuredRef
            };
        }
    }
}
