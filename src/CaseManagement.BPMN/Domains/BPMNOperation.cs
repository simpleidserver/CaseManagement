using System.Collections.Generic;

namespace CaseManagement.BPMN.Domains
{
    public class BPMNOperation
    {
        public BPMNOperation()
        {
            Parts = new List<BPMNMessageElement>();
        }

        public string Name { get; set; }
        public ICollection<BPMNMessageElement> Parts { get; set; }
    }
}
