using System;

namespace CaseManagement.BPMN.Domains
{
    [Serializable]
    public class BaseElement
    {
        public string Id { get; set; }

        public void FeedElt(BaseElement elt)
        {
            elt.Id = Id;
        }
    }
}
