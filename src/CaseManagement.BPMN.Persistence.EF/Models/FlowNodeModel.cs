using CaseManagement.BPMN.Domains;

namespace CaseManagement.BPMN.Persistence.EF.Models
{
    public class FlowNodeModel
    {
        public long Id { get; set; }
        public FlowNodeTypes Type { get; set; }
        public string Name { get; set; }
        public string SerializedContent { get; set; }
    }
}
