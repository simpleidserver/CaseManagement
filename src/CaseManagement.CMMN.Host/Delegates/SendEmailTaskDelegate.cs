using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Infrastructure;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Host.Delegates
{
    public class SendEmailTaskDelegate : WorkflowTaskDelegate
    {
        public override Task Handle(ProcessFlowInstance context)
        {
            return Task.FromResult(0);
        }
    }
}
