namespace CaseManagement.BPMN.Domains
{
    public class EndEvent : BaseCatchEvent
    {
        public override FlowNodeTypes FlowNode => FlowNodeTypes.ENDEVENT;

        public override object Clone()
        {
            var result = new EndEvent();
            FeedCatchEvent(result);
            return result;
        }

        public static new EndEvent Deserialize(string json)
        {
            return BaseCatchEvent.Deserialize<EndEvent>(json);
        }
    }
}
