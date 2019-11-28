using CaseManagement.Workflow.Domains;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure
{
    public abstract class WorkflowTaskDelegate
    {
        public abstract Task Handle(ProcessFlowInstance instance);
    }
}