using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public class ProcessTask : CMMNTask
    {
        public ProcessTask(string name) : base(name)
        {
            Mappings = new List<ParameterMapping>();
        }

        /// <summary>
        /// Zero or more ParameterMapping objects. A ParameterMapping of a ProcessTask specifies how an input of the ProcessTask is mapped to an
        /// input of the called Process and how an output of the called Process is mapped to an output of the ProcessTask
        /// </summary>
        public ICollection<ParameterMapping> Mappings { get; set; }
        /// <summary>
        /// A reference to a Process. If ProcessRef is not specified then processRefExpression must be specified. [1..1]
        /// </summary>
        public string ProcessRef { get; set; }
        /// <summary>
        /// A reference to a CaseFileItem.
        /// </summary>
        public string SourceRef { get; set; }
        /// <summary>
        /// If processRefExpression is specified, it is assumed that the expression evaluates to a QName which is a valid QName of an existing Process. [0...1]
        /// </summary>
        public CMMNExpression ProcessRefExpression { get; set; }

        public override object CloneTask()
        {
            return new ProcessTask(Name)
            {
                IsBlocking = IsBlocking,
                Mappings = Mappings.Select(m =>(ParameterMapping)m.Clone()).ToList(),
                ProcessRef = ProcessRef,
                SourceRef = SourceRef,
                ProcessRefExpression = ProcessRefExpression == null ? null : (CMMNExpression)ProcessRefExpression.Clone(),
                Name = Name
            };
        }
    }
}
