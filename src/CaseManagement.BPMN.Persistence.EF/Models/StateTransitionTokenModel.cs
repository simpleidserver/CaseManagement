namespace CaseManagement.BPMN.Persistence.EF.Models
{
    public class StateTransitionTokenModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string FlowNodeInstanceId { get; set; }
        public string SerializedContent { get; set; }
    }
}
