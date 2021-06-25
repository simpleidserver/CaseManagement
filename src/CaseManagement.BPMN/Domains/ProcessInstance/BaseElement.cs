using System;

namespace CaseManagement.BPMN.Domains
{
    [Serializable]
    public class BaseElement
    {
        public string EltId { get; set; }

        public void FeedElt(BaseElement elt)
        {
            elt.EltId = EltId;
        }
    }
}
