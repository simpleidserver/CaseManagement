using System.Collections.Generic;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNStageDefinition : CMMNWorkflowElementDefinition
    {
        public CMMNStageDefinition(string id, string name) : base(id, name)
        {
            Type = CMMNWorkflowElementTypes.Stage;
            Elements = new List<CMMNWorkflowElementDefinition>();
        }

        public ICollection<CMMNWorkflowElementDefinition> Elements { get; set; }
    }
}
