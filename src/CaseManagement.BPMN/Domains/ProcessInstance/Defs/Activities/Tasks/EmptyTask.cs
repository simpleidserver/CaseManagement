namespace CaseManagement.BPMN.Domains
{
    public class EmptyTask : BaseTask
    {
        public EmptyTask() : base() { }

        public override FlowNodeTypes FlowNode => FlowNodeTypes.EMPTYTASK;

        public override object Clone()
        {
            var result = new EmptyTask();
            FeedActivity(result);
            return result;
        }
    }
}
