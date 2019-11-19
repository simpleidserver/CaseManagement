using CaseManagement.Workflow.Engine;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure
{
    public abstract class WorkflowTaskDelegate
    {
        public abstract Task Handle(ProcessFlowInstanceExecutionContext context);
    }
}
