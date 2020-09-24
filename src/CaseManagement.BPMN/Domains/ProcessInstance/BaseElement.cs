namespace CaseManagement.BPMN.Domains
{
    public class BaseElement
    {
        public string Id { get; set; }

        public void FeedElt(BaseElement elt)
        {
            elt.Id = Id;
        }
    }
}
