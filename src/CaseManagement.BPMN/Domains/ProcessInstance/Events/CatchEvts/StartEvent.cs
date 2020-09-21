namespace CaseManagement.BPMN.Domains
{
    /// <summary>
    /// Indicates where a Process will start.
    /// </summary>
    public class StartEvent : BaseCatchEvent
    {
        public override object Clone()
        {
            var result = new StartEvent();
            FeedCatchEvent(result);
            return result;
        }
    }
}
