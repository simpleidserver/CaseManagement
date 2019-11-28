using CaseManagement.Workflow.Domains;
using System;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Engine
{
    public abstract class BaseProcessFlowElementProcessor : IProcessFlowElementProcessor
    {
        public abstract Type ProcessFlowElementType { get; }

        public async Task Handle(ProcessFlowInstance pf, ProcessFlowInstanceElement pfe)
        {
            pf.LaunchElement(pfe);
            if (await HandleProcessFlowInstance(pf, pfe))
            {
                pf.CompleteElement(pfe);
            }
        }

        protected abstract Task<bool> HandleProcessFlowInstance(ProcessFlowInstance pf, ProcessFlowInstanceElement pfe);
    }
}
