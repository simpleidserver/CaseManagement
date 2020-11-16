using System.Collections.Generic;

namespace CaseManagement.BPMN.Persistence.EF.Models
{
    public class BPMNInterfaceModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ImplementationRef { get; set; }
        public virtual ICollection<OperationModel> Operations { get; set; }
    }
}
