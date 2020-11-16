namespace CaseManagement.BPMN.Persistence.EF.Models
{
    public class SequenceFlowModel
    {
        public long Id { get; set; }
        public string EltId { get; set;}
        public string Name { get; set; }
        public string SourceRef { get; set; }
        public string TargetRef { get; set; }
        public string ConditionExpression { get; set; }
    }
}
