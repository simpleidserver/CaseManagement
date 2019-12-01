using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNProcess : ICloneable
    {
        public CMMNProcess()
        {
            Inputs = new List<CMMNParameter>();
            Outputs = new List<CMMNParameter>();
        }

        /// <summary>
        /// Implementation type of the business process.
        /// </summary>
        public string ImplementationType { get; set; }
        /// <summary>
        /// Zero or more inputs of the business process.
        /// </summary>
        public ICollection<CMMNParameter> Inputs { get; set; }
        /// <summary>
        /// Zero or more outputs of the business process.
        /// </summary>
        public ICollection<CMMNParameter> Outputs { get; set; }
        /// <summary>
        /// Name of the process.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Concrete process to be used.
        /// </summary>
        public string ProcessRef { get; set; }

        public object Clone()
        {
            return new CMMNProcess
            {
                ImplementationType = ImplementationType,
                Name = Name,
                ProcessRef = ProcessRef,
                Inputs = Inputs.Select(i => (CMMNParameter)i.Clone()).ToList(),
                Outputs = Outputs.Select(i => (CMMNParameter)i.Clone()).ToList()
            };
        }
    }
}
