namespace CaseManagement.BPMN.Domains
{
    public class EmptyTask : BaseTask
    {
        public override object Clone()
        {
            var result = new EmptyTask();
            FeedActivity(result);
            return result;
        }
    }
}
