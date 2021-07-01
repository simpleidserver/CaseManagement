namespace CaseManagement.BPMN.Domains
{
    public class BoundaryEvent : BaseCatchEvent
    {
        public override FlowNodeTypes FlowNode => FlowNodeTypes.BOUNDARYEVENT;
        public string AttachedToRef { get; set; }

        public override object Clone()
        {
            var result = new BoundaryEvent();
            FeedCatchEvent(result);
            result.AttachedToRef = AttachedToRef;
            return result;
        }

        public static new BoundaryEvent Deserialize(string json)
        {
            return BaseCatchEvent.Deserialize<BoundaryEvent>(json);
        }
    }
}
