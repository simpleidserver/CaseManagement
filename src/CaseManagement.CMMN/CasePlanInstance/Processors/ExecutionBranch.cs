using CaseManagement.CMMN.Domains;
using CaseManagement.Common.Processors;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class ExecutionBranch : BaseExecutionBranch<CaseEltInstance>
    {
        public ExecutionBranch(int level) : base(level)
        {
        }
    }
}
