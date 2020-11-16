using CaseManagement.BPMN.Domains;

namespace CaseManagement.BPMN.Persistence.EF.Models
{
    public class ItemDefinitionModel
    {
        public long Id { get; set; }
        public ItemKinds ItemKind { get; set; }
        public bool IsCollection { get; set; }
        public string StructureRef { get; set; }
    }
}
