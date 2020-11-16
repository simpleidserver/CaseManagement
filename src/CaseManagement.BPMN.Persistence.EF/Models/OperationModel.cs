namespace CaseManagement.BPMN.Persistence.EF.Models
{
    public class OperationModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ImplementationRef { get; set; }
        public string InMessageRef { get; set; }
        public string OutMessageRef { get; set; }
    }
}
