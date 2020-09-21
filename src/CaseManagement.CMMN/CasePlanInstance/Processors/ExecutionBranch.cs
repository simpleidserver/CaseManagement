using CaseManagement.CMMN.Domains;
using CaseManagement.Common.Processors;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class ExecutionBranch : BaseExecutionBranch<BaseCasePlanItemInstance>
    {
        public ExecutionBranch(int level) : base(level)
        {
        }
    }
}
