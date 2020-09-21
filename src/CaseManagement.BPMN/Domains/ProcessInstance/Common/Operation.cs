using System;

namespace CaseManagement.BPMN.Domains
{
    public class Operation : ICloneable
    {
        public string Name { get; set; }
        public string ImplementationRef { get; set; }

        public object Clone()
        {
            return new Operation
            {
                ImplementationRef = ImplementationRef,
                Name = Name
            };
        }
    }
}
