using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class ProcessTaskElementInstance : BaseTaskOrStageElementInstance
    {
        public ProcessTaskElementInstance()
        {
            Mappings = new List<ParameterMapping>();
        }

        public override string Type { get => "processtask"; }
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

        public override object Clone()
        {
            var result = new ProcessTaskElementInstance();
            FeedCasePlanElement(result);
            FeedTaskOrStage(result);
            return result;
        }
    }
}
