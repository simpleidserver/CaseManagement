using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Infrastructure;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Acceptance.Tests.Delegates
{
    public class SendEmailTaskDelegate : WorkflowTaskDelegate
    {
        public override Task Handle(ProcessFlowInstance instance)
        {
            return Task.FromResult(0);
        }
    }
}
