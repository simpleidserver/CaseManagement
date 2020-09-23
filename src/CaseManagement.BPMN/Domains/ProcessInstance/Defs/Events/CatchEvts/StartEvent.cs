namespace CaseManagement.BPMN.Domains
{
    /// <summary>
    /// Indicates where a Process will start.
    /// </summary>
    public class StartEvent : BaseCatchEvent
    {
        public override FlowNodeTypes FlowNode => FlowNodeTypes.STARTEVENT;

        public override object Clone()
        {
            var result = new StartEvent();
            FeedCatchEvent(result);
            return result;
        }

        public static new StartEvent Deserialize(string json)
        {
            return BaseCatchEvent.Deserialize<StartEvent>(json);
        }
    }
}
