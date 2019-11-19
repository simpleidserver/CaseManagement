using CaseManagement.BPMN.Infrastructure;
using CaseManagement.Workflow.Engine;
using CaseManagement.Workflow.Infrastructure;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Tests.Delegates
{
    public class SetFirstNameDelegate : WorkflowTaskDelegate
    {
        public override Task Handle(ProcessFlowInstanceExecutionContext context)
        {
            context.SetVariable("firstName", "simpleidserver");
            return Task.FromResult(0);
        }
    }
}
