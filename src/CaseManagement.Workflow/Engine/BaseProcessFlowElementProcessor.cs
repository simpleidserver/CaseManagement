using CaseManagement.Workflow.Domains;
using System;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Engine
{
    public abstract class BaseProcessFlowElementProcessor : IProcessFlowElementProcessor
    {
        public abstract Type ProcessFlowElementType { get; }

        public async Task Handle(ProcessFlowInstanceElement pfe, ProcessFlowInstanceExecutionContext context)
        {
            pfe.Run();
            if (await HandleProcessFlowInstance(pfe, context))
            {
                pfe.Finish();
            }
        }

        protected abstract Task<bool> HandleProcessFlowInstance(ProcessFlowInstanceElement pfe, ProcessFlowInstanceExecutionContext context);
    }
}
