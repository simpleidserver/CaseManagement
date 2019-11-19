using CaseManagement.Workflow.Domains;

namespace CaseManagement.Workflow.Engine
{
    public interface IProcessFlowElementProcessorFactory
    {
        IProcessFlowElementProcessor Build(ProcessFlowInstanceElement elt);
    }
}
