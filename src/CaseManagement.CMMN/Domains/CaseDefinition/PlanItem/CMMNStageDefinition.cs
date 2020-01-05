using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNStageDefinition
    {
        public CMMNStageDefinition(string name)
        {
            Elements = new List<CMMNWorkflowElementDefinition>();
        }

        public string Name { get; set; }
        public ICollection<CMMNWorkflowElementDefinition> Elements { get; set; }
    }
}
