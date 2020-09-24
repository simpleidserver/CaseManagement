namespace CaseManagement.BPMN.Domains
{
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