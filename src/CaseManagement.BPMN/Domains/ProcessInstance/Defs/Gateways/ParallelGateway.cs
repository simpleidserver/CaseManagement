namespace CaseManagement.BPMN.Domains
{
    public class ParallelGateway : BaseGateway
    {
        public override FlowNodeTypes FlowNode => FlowNodeTypes.PARALLELGATEWAY;

        public override object Clone()
        {
            var result = new ParallelGateway();
            FeedGateway(result);
            return result;
        }
    }
}
