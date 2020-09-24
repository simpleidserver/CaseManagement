namespace CaseManagement.BPMN.Domains
{
    public abstract class BaseGateway : BaseFlowNode
    {
        public GatewayDirections GatewayDirection { get; set; }

        protected void FeedGateway(BaseGateway gateway)
        {
            FeedFlowNode(gateway);
            gateway.GatewayDirection = GatewayDirection;
        }
    }
}
