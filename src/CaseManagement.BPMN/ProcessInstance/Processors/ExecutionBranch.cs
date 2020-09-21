using CaseManagement.BPMN.Domains;
using CaseManagement.Common.Processors;

namespace CaseManagement.BPMN.ProcessInstance.Processors
{
    public class ExecutionBranch : BaseExecutionBranch<BaseFlowNode>
    {
        public ExecutionBranch(int level) : base(level)
        {
        }
    }
}