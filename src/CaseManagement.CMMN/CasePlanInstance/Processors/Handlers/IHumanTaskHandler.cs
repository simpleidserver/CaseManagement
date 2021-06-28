using CaseManagement.CMMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors.Handlers
{
    public interface IHumanTaskHandler
    {
        string Implementation { get; }
        Task<string> Create(CMMNExecutionContext cmmnExecutionContext, CaseEltInstance elt, CancellationToken token);
    }
}
