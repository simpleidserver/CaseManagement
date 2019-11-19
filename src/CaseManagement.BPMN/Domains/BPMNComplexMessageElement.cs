using System.Collections.Generic;

namespace CaseManagement.BPMN.Domains
{
    public class BPMNComplexMessageElement : BPMNMessageElement
    {
        public BPMNComplexMessageElement()
        {
            Items = new List<BPMNMessageElement>();
        }

        public ICollection<BPMNMessageElement> Items { get; set; }
    }
}
