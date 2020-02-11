using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors.CaseFileItem
{
    public interface ICaseFileItemListener
    {
        string CaseFileItemType { get; }
        Task Start(ProcessorParameter parameter, CancellationToken token);
    }
}
