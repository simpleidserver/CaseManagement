namespace CaseManagement.BPMN.Domains
{
    public class ExclusiveGateway : BaseGateway
    {
        /// <summary>
        /// The Sequence Flow that will receive a token when none of the conditionExpressions on other outgoing Sequence Flows evaluate to true. 
        /// </summary>
        public string Default { get; set; }

        public override FlowNodeTypes FlowNode => FlowNodeTypes.EXCLUSIVEGATEWAY;

        public override object Clone()
        {
            var result = new ExclusiveGateway
            {
                Default = Default
            };
            FeedGateway(result);
            return result;
        }
    }
}
