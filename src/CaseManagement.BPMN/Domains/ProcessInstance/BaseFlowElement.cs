using System;

namespace CaseManagement.BPMN.Domains
{
    [Serializable]
    public class BaseFlowElement : BaseElement
    {
        public string Name { get; set; }

        protected void FeedFlowElt(BaseFlowElement elt)
        {
            FeedElt(elt);
            elt.Name = Name;
        }
    }
}