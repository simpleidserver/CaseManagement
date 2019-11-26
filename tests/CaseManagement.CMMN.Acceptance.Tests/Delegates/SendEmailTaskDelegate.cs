using System.Threading.Tasks;
using CaseManagement.Workflow.Engine;
using CaseManagement.Workflow.Infrastructure;

namespace CaseManagement.CMMN.Acceptance.Tests.Delegates
{
    public class SendEmailTaskDelegate : WorkflowTaskDelegate
    {
        public override Task Handle(ProcessFlowInstanceExecutionContext context)
        {
            return Task.FromResult(0);
        }
    }
}
