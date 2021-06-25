using Newtonsoft.Json;
using System;

namespace CaseManagement.BPMN.Domains
{
    public abstract class BaseFlowNode : BaseFlowElement, ICloneable
    {
        public abstract FlowNodeTypes FlowNode { get; }

        protected void FeedFlowNode(BaseFlowNode node)
        {
            FeedFlowElt(node);
        }

        public abstract object Clone();
    }
}
